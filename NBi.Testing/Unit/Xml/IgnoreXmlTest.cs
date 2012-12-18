using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class IgnoreXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.IgnoreXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_IgnoreAttributeNotSpecified_NotIgnored()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Ignore, Is.False);
        }

        [Test]
        public void Deserialize_IgnoreAttributeSetToTrue_Ignored()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Ignore, Is.True);
        }

        [Test]
        public void Deserialize_IgnoreElementAvailable_Ignored()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Ignore, Is.True);
        }

        [Test]
        public void Deserialize_IgnoreReasonFilled_IgnoreReasonLoaded()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].IgnoreReason, Is.EqualTo("The reason to ignore this test."));
        }

    }
}
