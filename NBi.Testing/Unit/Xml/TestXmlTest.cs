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

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class TestXmlTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TestXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_TestSuiteLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Name, Is.EqualTo("The TestSuite"));
        }
  

        [Test]
        public void Deserialize_SampleFile_TestsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests.Count, Is.GreaterThan(2));
        }
        [Test]
        public void Deserialize_SampleFile_TestMembersLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Name, Is.EqualTo("My first test case"));
            Assert.That(ts.Tests[0].UniqueIdentifier, Is.EqualTo("0001"));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintsLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].Constraints.Count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintSyntacticallyCorrectLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            ts.Tests.GetRange(0,2).ForEach(t => Assert.That(t.Constraints[0], Is.InstanceOf<SyntacticallyCorrectXml>()));

        }

        [Test]
        public void Deserialize_SampleFile_ConstraintFasterThanLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[1].Constraints[1], Is.InstanceOf<FasterThanXml>());
            Assert.That(((FasterThanXml)ts.Tests[1].Constraints[1]).MaxTimeMilliSeconds, Is.EqualTo(5000));
        }

        [Test]
        public void Deserialize_SampleFile_ConstraintEqualToLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[2].Constraints[0], Is.InstanceOf<EqualToXml>());
            //Assert.That(((EqualToXml)ts.Tests[2].Constraints[0]).ResultSetFile, Is.Not.Empty);
        }
        
        

        [Test]
        public void Deserialize_SampleFile_TestCategoriesLoaded()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

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
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainXml>());
            Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Not, Is.EqualTo(true));
        }

        [Test]
        [Ignore("Timeout attribute is not interpreted by NBi")]
        public void Deserialize_SampleFile_TimeoutAttributeCorrectlyRead()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Timeout, Is.TypeOf<int>());
            Assert.That(ts.Tests[testNr].Timeout, Is.EqualTo(1000));
        }

        [Test]
        public void Serialize_StructureXml_NoDefaultAndSettings()
        {
            var references = new List<ReferenceXml>() 
                    { new ReferenceXml() 
                        { Name = "Bob", ConnectionString = "connStr" } 
                    };
            var perspectiveXml = new PerspectiveXml();
            perspectiveXml.Caption = "My Caption";
            perspectiveXml.Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = "connStr" };
            perspectiveXml.Settings = new SettingsXml()
            {
                References = references
            };
            var structureXml = new StructureXml() 
            {
                Item = perspectiveXml,
                Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = "connStr" },
                Settings = new SettingsXml() {References= references}
            };
            var testXml = new TestXml()
            {
                Systems = new List<AbstractSystemUnderTestXml>() 
                    { 
                        structureXml
                    }
            };

            var serializer = new XmlSerializer(typeof(TestXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("My Caption"));
            Assert.That(content, Is.Not.StringContaining("efault"));
            Assert.That(content, Is.Not.StringContaining("eference"));
        }

    }
}
