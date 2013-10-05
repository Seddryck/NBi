#region Using directives
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class StructureXmlTest
    {


        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.StructureXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_Hierarchy()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            
            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchyXml>());

            HierarchyXml item = (HierarchyXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Caption, Is.EqualTo("hierarchy"));
            Assert.That(item.Dimension, Is.EqualTo("dimension"));
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.GetConnectionString(), Is.EqualTo("ConnectionString"));
        }


        [Test]
        public void GetAutoCategories_Hierarchy_ValidList()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

            Assert.That(autoCategories, Has.Member("Dimension 'dimension'"));
            Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
            Assert.That(autoCategories, Has.Member("Hierarchy 'hierarchy'"));
            Assert.That(autoCategories, Has.Member("Hierarchies"));
            Assert.That(autoCategories, Has.Member("Structure"));
        }

        [Test]
        public void GetAutoCategories_MeasureGroup_ValidList()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var autoCategories = ts.Tests[testNr].Systems[0].GetAutoCategories();

            Assert.That(autoCategories, Has.Member("Measure group 'MeasureGroupName'"));
            Assert.That(autoCategories, Has.Member("Perspective 'Perspective'"));
            Assert.That(autoCategories, Has.Member("Measure groups"));
            Assert.That(autoCategories, Has.Member("Structure"));
        }

        [Test]
        public void Deserialize_SampleFile_MeasureGroup()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<MeasureGroupXml>());

            MeasureGroupXml item = (MeasureGroupXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Perspective, Is.EqualTo("Perspective"));
            Assert.That(item.GetConnectionString(), Is.EqualTo("ConnectionString"));
        }

        [Test]
        public void Serialize_StructureXml_NoDefaultAndSettings()
        {
            var perspectiveXml = new PerspectiveXml();
            perspectiveXml.Caption = "My Caption";
            perspectiveXml.Default = new DefaultXml() { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = "connStr" };
            perspectiveXml.Settings = new SettingsXml()
            {
                References = new List<ReferenceXml>() 
                    { new ReferenceXml() 
                        { Name = "Bob", ConnectionString = "connStr" } 
                    }
            };
            var structureXml = new StructureXml() { Item = perspectiveXml };

            var serializer = new XmlSerializer(typeof(StructureXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, structureXml);
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
