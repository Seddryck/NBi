using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Xml.Systems.Structure;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class MeasureDeserialize
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.MeasureTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_MeasureGroupLoaded()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MeasureXml>());
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).MeasureGroup, Is.EqualTo("measure-group"));
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).Specification.IsMeasureGroupSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_MeasureGroupNotSpecified()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MeasureXml>());
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).MeasureGroup, Is.Null.Or.Empty);
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).Specification.IsMeasureGroupSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_DisplayFolderLoaded()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MeasureXml>());
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).DisplayFolder, Is.EqualTo("display-folder"));
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).Specification.IsDisplayFolderSpecified, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_DisplayFolderNotSpecified()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<MeasureXml>());
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).DisplayFolder, Is.Null.Or.Empty);
            Assert.That(((MeasureXml)ts.Tests[testNr].Systems[0]).Specification.IsMeasureGroupSpecified, Is.False);
        }
    }
}
