using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class CountXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.CountXmlTestSuite.xml"))
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
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).ExactlySpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThanSpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThanSpecified, Is.False);
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
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).ExactlySpecified, Is.False);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).LessThanSpecified, Is.True);
            Assert.That(((CountXml)ts.Tests[testNr].Constraints[0]).MoreThanSpecified, Is.True);
        }

        [Test]
        public void Serialize_OnlyExactlySpecified_MoreThanLessThanNotSet()
        {
            var count = new CountXml();
            count.Exactly = 10;

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CountXml>(count);

            Assert.That(xml, Is.StringContaining("exactly"));
            Assert.That(xml, Is.Not.StringContaining("more-than"));
            Assert.That(xml, Is.Not.StringContaining("less-than"));
        }

        [Test]
        public void Serialize_LessThanSpecified_LessThanSet()
        {
            var count = new CountXml();
            count.LessThan = 10;

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<CountXml>(count);

            Assert.That(xml, Is.Not.StringContaining("exactly"));
            Assert.That(xml, Is.Not.StringContaining("more-than"));
            Assert.That(xml, Is.StringContaining("less-than"));
        }
    }
}
