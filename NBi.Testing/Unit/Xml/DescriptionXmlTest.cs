using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class DescriptionXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.DescriptionXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_DescriptionAttributeNotSpecified_NoDescription()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Description, Is.Empty);
        }

        [Test]
        public void Deserialize_DescriptionAttributeSet_DescriptionAvailable()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Description, Is.EqualTo("Test's description"));
        }

        [Test]
        public void Deserialize_DescriptionElementAvailable_DescriptionAvailable()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Description, Is.EqualTo("Test's description"));
        }
    }
}
