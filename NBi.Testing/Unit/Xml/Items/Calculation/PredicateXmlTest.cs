using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Testing.Unit.Xml.Items.Calculation
{
    public class PredicationXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.PredicateXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_OnlyOperandNoName_PredicateXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            Assert.That(ctr.Predication, Is.Not.Null);
            Assert.That(ctr.Predication.Operand, Is.EqualTo("ModDepId"));
        }

        [Test]
        public void Deserialize_OnlyNameNoOperand_PredicateXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            Assert.That(ctr.Predication, Is.Not.Null);
            Assert.That(ctr.Predication.Operand, Is.EqualTo("ModDepId"));
        }

        [Test]
        public void Deserialize_CorrectPredicate_PredicateXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            Assert.That(ctr.Predication, Is.Not.Null);
            Assert.That(ctr.Predication.Predicate, Is.TypeOf<MoreThanXml>());
        }

        [Test]
        public void Serialize_PredicateXml_OnlyOperandNoName()
        {
            var allRowsXml = new AllRowsXml
            {
                Predication = new PredicationXml()
                {
                    Operand = "#1",
                    Predicate = new FalseXml()
                }
            };

            var serializer = new XmlSerializer(typeof(AllRowsXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, allRowsXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("operand"));
            Assert.That(content, Is.Not.StringContaining("name"));
        }

        [Test]
        public void Serialize_ModuloXml_AllPredicateInfoCorrectlySerialized()
        {
            var allRowsXml = new AllRowsXml
            {
                Predication = new PredicationXml()
                {
                    Operand = "#1",
                    Predicate = new ModuloXml() { SecondOperand = "10", Value = "5" }
                }
            };

            var serializer = new XmlSerializer(typeof(AllRowsXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, allRowsXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<modulo"));
            Assert.That(content, Is.StringContaining("second-operand=\"10\""));
            Assert.That(content, Is.StringContaining(">5<"));
        }
    }
}
