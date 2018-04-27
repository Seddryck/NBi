using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Systems;
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

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class ReferenceExistsXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ReferenceExistsXmlTestSuite.xml"))
            using (var reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyReferenceExists()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ReferenceExistsXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMapping()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var refExists = ts.Tests[testNr].Constraints[0] as ReferenceExistsXml;
            var mappings = refExists.Mappings;

            Assert.That(mappings, Has.Count.EqualTo(1));
            Assert.That(mappings[0].Child, Is.EqualTo("GroupId"));
            Assert.That(mappings[0].Parent, Is.EqualTo("Id"));
            Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Numeric));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMappings()
        {
            int testNr = 1;

            TestSuiteXml ts = DeserializeSample();
            var refExists = ts.Tests[testNr].Constraints[0] as ReferenceExistsXml;
            var mappings = refExists.Mappings;

            Assert.That(mappings, Has.Count.EqualTo(2));
        }

        [Test]
        public void Serialize_ReferenceExistsXml_Correct()
        {
            var refExistsXml = new ReferenceExistsXml()
            {
                Mappings = new List<ColumnMappingXml>()
                {
                    new ColumnMappingXml() {Child = "#1", Parent="Col1", Type=ColumnType.Numeric},
                    new ColumnMappingXml() {Child = "#0", Parent="Col2", Type=ColumnType.Text}
                },
                ResultSet = new ResultSetSystemXml()
            };

            var serializer = new XmlSerializer(typeof(ReferenceExistsXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, refExistsXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("column-mapping"));
            Assert.That(content, Is.StringContaining("parent"));
            Assert.That(content, Is.StringContaining("type"));
            Assert.That(content, Is.StringContaining("numeric"));
        }
    }
}
