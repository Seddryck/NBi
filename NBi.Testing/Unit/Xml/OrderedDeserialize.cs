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
    public class OrderedDeserialize
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.OrderedTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalNothingSpecified()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Alphabetical));
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Descending, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalDescendingSpecified()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Descending, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintChronologicalSpecified()
        {
            int testNr = 2;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Chronological));
        }

        [Test]
        public void Deserialize_SampleFile_OrderedConstraintAlphabeticalSpecificSpecified()
        {
            int testNr = 3;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<OrderedXml>());
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Rule, Is.EqualTo(OrderedXml.Order.Specific));
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Definition, Has.Count.EqualTo(3));
            Assert.That(((OrderedXml)ts.Tests[testNr].Constraints[0]).Definition[0], Is.EqualTo("Leopold"));
        }

        [Test]
        public void Deserialize_SampleFile_ContainsNotAttributeCorrectlyRead()
        {
            int testNr = 4;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainsXml>());
            Assert.That(((ContainsXml)ts.Tests[testNr].Constraints[0]).Not, Is.EqualTo(true));
        }

        [Test]
        public void Deserialize_SampleFile_IgnoreAttributeSetToTrue()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr], Is.TypeOf<TestXml>());
            Assert.That(ts.Tests[testNr].Ignore, Is.True);
        }

    }
}
