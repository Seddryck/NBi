using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
using System.Xml;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class TestXmlTest
{

    protected TestSuiteXml DeserializeSample()
    {
        // Declare an object variable of the type to be deserialized.
        var manager = new XmlManager();

        // A Stream is needed to read the XML document.
        using (var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.TestXmlTestSuite.xml")
                                       ?? throw new FileNotFoundException())
        using (var reader = new StreamReader(stream))
        {
            manager.Read(reader);
        }
        return manager.TestSuite!;
    }
    
    [Test]
    public void Deserialize_SampleFile_TestSuiteLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Name, Is.EqualTo("The TestSuite"));
    }


    [Test]
    public void Deserialize_SampleFile_TestsLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests.Count, Is.GreaterThan(2));
    }
    [Test]
    public void Deserialize_SampleFile_TestMembersLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[0].Name, Is.EqualTo("My first test case"));
        Assert.That(ts.Tests[0].UniqueIdentifier, Is.EqualTo("0001"));
    }

    [Test]
    public void Deserialize_SampleFile_ConstraintsLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[0].Constraints.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void Deserialize_SampleFile_ConstraintSyntacticallyCorrectLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        ts.Tests.GetRange(0,2).ForEach(t => Assert.That(t.Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>()));

    }

    [Test]
    public void Deserialize_SampleFile_ConstraintFasterThanLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[1].Constraints[1], Is.InstanceOf<FasterThanXml>());
        Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).MaxTimeMilliSeconds, Is.EqualTo(5000));
    }

    [Test]
    public void Deserialize_SampleFile_ConstraintEqualToLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[2].Constraints[0], Is.InstanceOf<EqualToXml>());
        //Assert.That(((EqualToXml)ts.Tests[2].Constraints[0]).ResultSetFile, Is.Not.Empty);
    }
    
    

    [Test]
    public void Deserialize_SampleFile_TestCategoriesLoaded()
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[1].Categories.Count, Is.EqualTo(2));
        Assert.That(ts.Tests[1].Categories, Has.Member("Category 1"));
        Assert.That(ts.Tests[1].Categories, Has.Member("Category 2"));
    }

    [Test]
    public void Deserialize_SampleFile_ContainNotAttributeCorrectlyRead()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainXml>());
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Not, Is.EqualTo(true));
    }

    [Test]
    [Ignore("Timeout attribute is not interpreted by NBi")]
    public void Deserialize_SampleFile_TimeoutAttributeCorrectlyRead()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Timeout, Is.TypeOf<int>());
        Assert.That(ts.Tests[testNr].Timeout, Is.EqualTo(1000));
    }

    [Test]
    public void Deserialize_SampleFile_SupportLargeUid()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].UniqueIdentifier, Is.EqualTo("45212"));
    }

    [Test]
    public void Deserialize_NotImplemented_GetAllInfo()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].UniqueIdentifier, Is.EqualTo("5"));
        Assert.That(ts.Tests[testNr].Categories, Has.Count.EqualTo(2));
        Assert.That(ts.Tests[testNr].NotImplemented.Reason, Is.EqualTo("Because we're not in version 1.18"));
        Assert.That(ts.Tests[testNr].Drafts, Has.Count.EqualTo(2));
    }

    [Test]
    public void Serialize_StructureXml_NoDefaultAndSettings()
    {
        var references = new List<ReferenceXml>() 
                { new ()
                    { Name = "Bob", ConnectionString = new ConnectionStringXml() { Inline = "connStr" } }
                };
        var perspectiveXml = new PerspectiveXml
        {
            Caption = "My Caption",
            Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
            Settings = new SettingsXml()
            {
                References = references
            }
        };
        var structureXml = new StructureXml() 
        {
            Item = perspectiveXml,
            Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = "connStr" } },
            Settings = new SettingsXml() {References= references}
        };
        var testXml = new TestXml()
        {
            Systems =
                [
                    structureXml
                ]
        };

        var serializer = new XmlSerializer(typeof(TestXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("My Caption"));
        Assert.That(content, Does.Not.Contain("efault"));
        Assert.That(content, Does.Not.Contain("eference"));
    }

    [Test]
    public void Serialize_NotImplemented_FullySerialized()
    {
        var doc = new XmlDocument();
        var nodes = doc.CreateElement("nodes");
        var assert = doc.CreateElement("assert");

        var testSuiteXml = new TestSuiteXml()
        {
            Tests =
            [
                new TestXml()
                {
                    Categories = ["My Category", "Not Implemented"],
                    NotImplemented = new IgnoreXml() { Reason = "My good reason"},
                    Drafts = [nodes, assert]
                }
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<test"));
        Assert.That(content, Does.Contain("<category"));
        Assert.That(content, Does.Not.Contain("<system-under-test"));
        Assert.That(content, Does.Contain("<category"));
        Assert.That(content, Does.Contain("<not-implemented"));
        Assert.That(content, Does.Contain("<nodes"));
        Assert.That(content, Does.Contain("<assert"));
    }

}
