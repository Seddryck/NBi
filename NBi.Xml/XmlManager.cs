using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NBi.Xml
{
    public class XmlManager
    {
        public TestSuiteXml TestSuite {get; protected set;}
        protected bool _isValid; 

        public XmlManager() { }

        public void Load(string filename)
        {
            if (!this.Validate(filename))
                throw new ArgumentException("The test suite is not valid. Check with the XSD");

            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(TestSuiteXml));
                        
            using (StreamReader reader = new StreamReader(filename))
            {
                // Use the Deserialize method to restore the object's state.
                TestSuite = (TestSuiteXml)serializer.Deserialize(reader);
            }
        }

        public void Persist(string filename, TestSuiteXml testSuite)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            var serializer = new XmlSerializer(typeof(TestSuiteXml));

            using (var writer = new StreamWriter(filename))
            {
                // Use the Serialize method to store the object's state.
                serializer.Serialize(writer, testSuite);
            }
        }

        protected bool Validate(string filename)
        {
            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            //Get the Schema
            // A Stream is needed to read the XSD document contained in the assembly.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Xml.NBi-TestSuite.xsd"))
            {
                settings.Schemas.Add("http://NBi/TestSuite", XmlReader.Create(stream));
                settings.Schemas.Compile();
            }

            _isValid = true;

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(filename, settings);

            // Parse the file. 
            while (reader.Read()) ;
            //The validationeventhandler is the only thing that would set _isValid to false

            return _isValid;
        }

        private void ValidationCallBack(Object sender, ValidationEventArgs args)
        {
            //This is only called on error
            // Display any warnings or errors.

            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("Warning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("Validation error: " + args.Message);

            _isValid = false; //Validation failed
        }

        public TestSuiteXml BuildTestSuite(string queriesDirectory, string resultSetsDirectory, string connectionString)
        {
            var testSuite = new TestSuiteXml();

            var queries = Directory.GetFiles(queriesDirectory);
            foreach (var query in queries)
            {
                if (File.Exists(Path.Combine(resultSetsDirectory, Path.GetFileNameWithoutExtension(query) + ".csv")))
                {
                    var test = new TestXml();

                    testSuite.Tests.Add(test);
                    test.Name = Path.GetFileNameWithoutExtension(query);
                    test.Categories.AddRange(Path.GetFileNameWithoutExtension(query).Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries));

                    var ctr = new EqualToXml();
                    test.Constraints.Add(ctr);
                    ctr.ResultSetFile=Path.Combine(resultSetsDirectory, Path.GetFileNameWithoutExtension(query) + ".csv");

                    var tc = new TestCaseXml();
                    test.TestCases.Add(tc);
                    tc.Filename = query;
                    tc.ConnectionString = connectionString;
                }
            }
            return testSuite;
        }

    }
}
