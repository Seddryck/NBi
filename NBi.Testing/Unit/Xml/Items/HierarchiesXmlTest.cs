using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class HierarchiesXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.HierarchiesXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_DimensionLoaded()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

            HierarchiesXml item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Dimension, Is.EqualTo("dimension"));
        }

        [Test]
        public void Deserialize_SampleFile_DisplayFolderLoaded()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

            HierarchiesXml item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.DisplayFolder, Is.EqualTo("display-folder"));
            Assert.That(item.Specification.IsDisplayFolderSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_DisplayFolderNotSpecified()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

            HierarchiesXml item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.DisplayFolder, Is.Null.Or.Empty);
            Assert.That(item.Specification.IsDisplayFolderSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_MeasureWithDisplayFolderRoot()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<HierarchiesXml>());

            HierarchiesXml item = (HierarchiesXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.DisplayFolder, Is.Empty);
            Assert.That(item.Specification.IsDisplayFolderSpecified, Is.True);
        }

               
    }
}
