using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class ColumnXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ColumnXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_TableAndPerspectiveLoaded()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)ts.Tests[testNr].Systems[0]).Item, Is.TypeOf<ColumnXml>());

            ColumnXml item = (ColumnXml)((StructureXml)ts.Tests[testNr].Systems[0]).Item;
            Assert.That(item.Caption, Is.EqualTo("column"));
            Assert.That(item.Table, Is.EqualTo("table"));
            Assert.That(item.Perspective, Is.EqualTo("perspective"));
        }

               
    }
}
