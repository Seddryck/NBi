using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class EvaluateRowsXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.EvaluateRowsXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ValidateType()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EvaluateRowsXml>());
        }

        [Test]
        public void Deserialize_SampleFile_Variable()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var ctr = ts.Tests[testNr].Constraints[0] as EvaluateRowsXml;
            Assert.That(ctr.Variables, Has.Count.EqualTo(3));
            Assert.That(ctr.Variables[0].Name, Is.EqualTo("OrderQuantity"));
            Assert.That(ctr.Variables[0].Column, Is.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_Expression()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var ctr = ts.Tests[testNr].Constraints[0] as EvaluateRowsXml;
            Assert.That(ctr.Expressions, Has.Count.EqualTo(1));
            Assert.That(ctr.Expressions[0].Value, Is.EqualTo("= OrderQuantity*(UnitPrice-(UnitPrice*UnitDiscount))"));
            Assert.That(ctr.Expressions[0].Column, Is.EqualTo(5));
            Assert.That(ctr.Expressions[0].Type, Is.EqualTo(ColumnType.Numeric));
            Assert.That(ctr.Expressions[0].Tolerance, Is.EqualTo("0.01"));
        }
    }
}
