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
    public class CountDeserialize
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.CountTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_CountExactly()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<CountXml>());
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Exactly, Is.EqualTo(10));
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsExactlySpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsLessThanSpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsMoreThanSpecified, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_CountMoreAndLess()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<CountXml>());
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThan, Is.EqualTo(10));
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThan, Is.EqualTo(15));
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsExactlySpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsLessThanSpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).Specification.IsMoreThanSpecified, Is.True);
        }
    }
}
