using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class ContainsXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ContainsXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ContainsCaptionNotIgnoringCaseImplicitely()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Caption, Is.EqualTo("xyz"));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsCaptionIgnoringCaseExplicitely()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Caption.ToLower(), Is.EqualTo("xyz"));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsExactly()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Exactly, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ContainsReadItems()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("xyz"));
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("abc"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsSubset()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Exactly, Is.False);
        }
    }
}
