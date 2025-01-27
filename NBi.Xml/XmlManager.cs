using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Settings;
using NBi.Xml.Decoration.Command;
using System.Text.RegularExpressions;
using NBi.Xml.SerializationOption;

namespace NBi.Xml;

public class XmlManager
{
    public virtual TestSuiteXml? TestSuite { get; protected set; }
    public virtual NameValueCollection ConnectionStrings { get; set; }
    private readonly IList<XmlSchemaException> validationExceptions;
    private readonly XmlDocument docXml;
    private string? basePath;

    public XmlManager()
    {
        docXml = new XmlDocument();
        ConnectionStrings = new NameValueCollection();
        validationExceptions = new List<XmlSchemaException>();
    }

    #region Load methods

    public virtual void Load(string testSuiteFilename)
    {
        Load(testSuiteFilename, null, false);
    }

    public virtual void Load(string testSuiteFilename, bool isDtdProcessing)
    {
        Load(testSuiteFilename, null, isDtdProcessing);
    }

    public virtual void Load(string testSuiteFilename, string? settingsFilename, bool isDtdProcessing)
    {
        //define the basePath
        basePath = Path.GetDirectoryName(testSuiteFilename) + Path.DirectorySeparatorChar;

        //ensure the file is existing
        if (!File.Exists(testSuiteFilename))
            throw new ArgumentException(string.Format("No test-suite has been found at the location '{0}'.", testSuiteFilename));

        using (var streamReader = new StreamReader(testSuiteFilename, Encoding.UTF8, true))
        {
            // Create the XmlReader object for validation
            using (var xmlReader = BuildXmlReader(streamReader, isDtdProcessing))
            {
                Read(xmlReader);
            }
        }

        //Load the settings optionally define in another file or in the config file.
        if (!string.IsNullOrEmpty(settingsFilename))
        {
            var fullPath = Path.IsPathRooted(settingsFilename) ? settingsFilename : basePath + settingsFilename;
            var settings = LoadSettings(fullPath);
            TestSuite!.Settings = settings;
        }
        else
        {
            TestSuite!.Settings.GetValuesFromConfig(ConnectionStrings);
        }

        //Apply the basePath
        TestSuite.Settings.BasePath = basePath;

        //Copy/Paste the default settings to each test.
        ApplyDefaultSettings();

        //We need to create a second object xmlReader for loading the docXml that will be used 
        //to display the test definition in the stacktrace.
        using (var streamReader = new StreamReader(testSuiteFilename, Encoding.UTF8, true))
        {
            using (var xmlReader = BuildXmlReader(streamReader, isDtdProcessing))
                docXml.Load(xmlReader);
        }
        ReassignXml();
    }

    #endregion

    #region Read methods

    internal void Read(StreamReader reader)
    {
        var xmlReader = BuildXmlReader(reader, false);
        Read(xmlReader);
    }

    protected virtual void Read(XmlReader reader)
    {
        //Add the attributes that should only be used during read phase
        //These attributes are kept for compatibility with previous versions
        //They should never been used during write process
        var attrs = new ReadOnlyAttributes();
        attrs.Build();

        // Create an instance of the XmlSerializer specifying type and read-attributes.
        try
        {
            validationExceptions.Clear();
            var serializer = new XmlSerializer(typeof(TestSuiteXml), attrs);

            // Use the Deserialize method to restore the object's state.
            TestSuite = (TestSuiteXml)(serializer.Deserialize(reader) ?? throw new NullReferenceException());
        }
        catch (InvalidOperationException ex)
        {
            if (ex.InnerException is XmlException)
            {
                XmlSchemaException xmlSchemaException;
                if (ex.InnerException.Message.Contains("For security reasons DTD is prohibited"))
                    xmlSchemaException = new XmlSchemaException("DTD is prohibited. To activate it, set the flag allow-dtd-processing to true in the config file associated to this test-suite");
                else
                {
                    var regex = new Regex(@"Line (\d+), position (\d+).$");
                    var match = regex.Match(ex.InnerException.Message);
                    if (match.Success)
                    {
                        int.TryParse(match.Groups[1].Value, out var line);
                        int.TryParse(match.Groups[2].Value, out var position);
                        xmlSchemaException = new XmlSchemaException(ex.InnerException.Message, ex, line, position);
                    }
                    else
                        xmlSchemaException = new XmlSchemaException(ex.InnerException.Message);

                }
                Console.WriteLine(xmlSchemaException.Message);
                validationExceptions.Add(xmlSchemaException);

            }
            else
                ParseCascadingInvalidOperationException((InvalidOperationException)ex.InnerException!);
        }

        if (validationExceptions.Count > 0)
        {
            var message = "The test suite is not valid. Check with the XSD.";
            message += string.Format(" {0} error{1} {2} been found during the validation of the test-suite:\r\n"
                                            , validationExceptions.Count
                                            , validationExceptions.Count > 1 ? "s" : string.Empty
                                            , validationExceptions.Count > 1 ? "have" : "has");

            foreach (var error in validationExceptions)
                message += $"\tAt line {error.LineNumber}: {error.Message}\r\n";

            throw new ArgumentException(message);
        }
    }

    private void ParseCascadingInvalidOperationException(InvalidOperationException exception)
    {
        if (exception == null)
            return;

        Console.WriteLine(exception.Message);
        ParseCascadingInvalidOperationException((InvalidOperationException)exception.InnerException!);
    }

    #endregion

    #region BuildXmlReader methods

    /// <summary>
    /// Protected methods to build an XmlReaderSettings with the expected values for xml validation, dtd processing, Url Resolution, schemas settings
    /// </summary>
    /// <param name="isDtdProcessing">define if dtd prcessingis allowed</param>
    /// <returns>An XmlReaderSettings correctly defined </returns>
    private XmlReaderSettings BuildXmlReaderBaseSettings(bool isDtdProcessing)
    {
        // Set the validation settings.
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            DtdProcessing = isDtdProcessing ? DtdProcessing.Parse : DtdProcessing.Prohibit,
            XmlResolver = new LocalXmlUrlResolver(basePath ?? string.Empty)
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            }
        };
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationEventHandler += delegate (object? sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("Validation warning: " + args.Message);
            else
                Console.WriteLine("Validation error: " + args.Message);

            validationExceptions.Add(args.Exception);
        };

        return settings;
    }

    private XmlSchemaSet AddSchemas(IEnumerable<string> schemas, string targetNamespace)
    {
        var schemaSet = new XmlSchemaSet();
        foreach (var ressourceName in schemas)
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(ressourceName)
                                           ?? throw new FileNotFoundException()
                                           )
                schemaSet.Add(targetNamespace, XmlReader.Create(stream));
        schemaSet.Compile();
        return schemaSet;
    }

    protected XmlReader BuildXmlReader(StreamReader reader, bool isDtdProcessing)
    {
        var xmlReaderSettings = BuildXmlReaderBaseSettings(isDtdProcessing);

        //define XSD schemas to add 
        var schemaSet = AddSchemas(new[]
                                        {"NBi.Xml.Schema.BaseType.xsd"
                                        , "NBi.Xml.Schema.Cleanup.xsd"
                                        , "NBi.Xml.Schema.Setup.xsd"
                                        , "NBi.Xml.Schema.Test.xsd"
                                        , "NBi.Xml.Schema.Settings.xsd"
                                        , "NBi.Xml.Schema.TestSuite.xsd"}
                                    , "http://NBi/TestSuite");

        xmlReaderSettings.Schemas = schemaSet;

        var xmlReader = XmlReader.Create(reader, xmlReaderSettings);
        return xmlReader;
    }

    #endregion

    #region Load settings

    protected virtual SettingsXml LoadSettings(string settingsFilename)
    {
        //ensure the file is existing
        if (!File.Exists(settingsFilename))
            throw new ArgumentException(string.Format("The file '{0}' has been referenced for settings by the configuration file but this file hasn't been not found!", settingsFilename));

        //Create an empty XmlRoot.
        //This is needed because the class settingsXml is not decorated with an attribute "XmlRoot".
        var xmlRoot = new XmlRootAttribute
        {
            ElementName = "settings",
            Namespace = "http://NBi/TestSuite",
            IsNullable = true
        };

        // Create the XmlReader object.
        using (var xmlReader = BuildXmlReaderForSettings(settingsFilename, false))
        {
            var overrides = new ReadOnlyAttributes();
            overrides.Build();

            // Create an instance of the XmlSerializer specifying type.
            var serializer = new XmlSerializer(typeof(SettingsXml), overrides, null, xmlRoot, string.Empty);
            // Use the Deserialize method to restore the object's state.
            return (SettingsXml)(serializer.Deserialize(xmlReader) ?? throw new NullReferenceException());
        }
    }

    protected XmlReader BuildXmlReaderForSettings(string filename, bool isDtdProcessing)
    {
        var xmlReaderSettings = BuildXmlReaderBaseSettings(isDtdProcessing);

        //define XSD schemas to add 
        var schemaSet = AddSchemas(new[]
                                        {"NBi.Xml.Schema.BaseType.xsd"
                                        , "NBi.Xml.Schema.Settings.xsd"}
                                    , "http://NBi/TestSuite");

        xmlReaderSettings.Schemas = schemaSet;

        var xmlReader = XmlReader.Create(filename, xmlReaderSettings);
        return xmlReader;
    }

    #endregion

    #region ApplyDefaultSettings methods
    internal void ApplyDefaultSettings()
    {
        //Apply defaults
        foreach (var test in TestSuite!.GetAllTests())
            ApplyDefaultSettings(test);
    }

    private void ApplyDefaultSettings(TestXml test)
    {
        if (TestSuite!.Settings is not null)
        {
            foreach (var sut in test.Systems)
            {
                sut.Default = TestSuite.Settings.GetDefault(Settings.SettingsXml.DefaultScope.SystemUnderTest);
                sut.Settings = TestSuite.Settings;
                if (sut is IReferenceFriendly sutRF)
                    sutRF.AssignReferences(TestSuite.Settings.References);

            }
            foreach (var ctr in test.Constraints)
            {
                ctr.Default = TestSuite.Settings.GetDefault(SettingsXml.DefaultScope.Assert);
                ctr.Settings = TestSuite.Settings;
                if (ctr is IReferenceFriendly ctrRF)
                    ctrRF.AssignReferences(TestSuite.Settings.References);
            }
        }
        var decorationCommands = new List<DecorationCommandXml>();
        decorationCommands.AddRange(test.Setup.Commands);
        decorationCommands.AddRange(test.Cleanup.Commands);
        foreach (var cmd in decorationCommands)
        {
            if (TestSuite.Settings != null)
            {
                cmd.Settings = TestSuite.Settings;
                if (cmd is IReferenceFriendly referenceFriendlyCmd)
                    referenceFriendlyCmd.AssignReferences(TestSuite.Settings.References);
            }
        }
    }

    #endregion

    protected internal void ReassignXml()
    {
        //Get the Xml content of the tests define in the testSuite
        var testNodes = docXml.GetElementsByTagName("test");
        for (int i = 0; i < TestSuite!.Tests.Count; i++)
        {
            //Add indentation and line breaks
            var nodeXml = new XmlDocument();
            nodeXml.LoadXml(testNodes[i]!.OuterXml);
            var content = XmlBeautifier.Beautify(nodeXml);
            //Add the content to the test (Used for StackTrace)
            TestSuite.Tests[i].Content = content;
        }
    }

    public void Persist(string filename, TestSuiteXml testSuite)
    {
        //Overrides some attributes sepcifically for writting (mostly enforce CDATA)
        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        // Create an instance of the XmlSerializer specifying type and namespace.
        var serializer = new XmlSerializer(typeof(TestSuiteXml), overrides);

        using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
        {
            // Use the Serialize method to store the object's state.
            serializer.Serialize(writer, testSuite);
        }
        //Debug.Write(XmlSerializeFrom<TestSuiteXml>(testSuite));
    }

    protected internal string XmlSerializeFrom<T>(T objectData)
        => SerializeFrom(objectData!, typeof(T));

    protected string SerializeFrom(object objectData, Type type)
    {
        var serializer = new XmlSerializer(type);
        var result = string.Empty;
        using (var writer = new StringWriter())
        {
            // Use the Serialize method to store the object's state.
            serializer.Serialize(writer, objectData);
            result = writer.ToString();
        }
        return result;
    }

    protected internal string XmlSerializeFrom<T>(T objectData, ReadWriteAttributes attr)
        => SerializeFrom(objectData!, typeof(T), attr);

    protected string SerializeFrom(object objectData, Type type, ReadWriteAttributes attr)
    {
        var serializer = new XmlSerializer(type, attr);
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, objectData);
            return writer.ToString();
        }
    }

    protected internal T XmlDeserializeTo<T>(string objectData)
        => (T)DeserializeTo(objectData, typeof(T));

    protected object DeserializeTo(string objectData, Type type)
    {
        var serializer = new XmlSerializer(type);
        using var reader = new StringReader(objectData);
        return serializer.Deserialize(reader) ?? throw new NullReferenceException();
    }

    protected internal T XmlDeserializeTo<T>(string objectData, ReadWriteAttributes attr)
        => (T)DeserializeTo(objectData, typeof(T), attr);

    protected object DeserializeTo(string objectData, Type type, ReadWriteAttributes attr)
    {
        var serializer = new XmlSerializer(type, attr);
        using var reader = new StringReader(objectData);
        return serializer.Deserialize(reader) ?? throw new NullReferenceException();
    }
}
