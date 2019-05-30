using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Xml.Variables;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using NBi.Xml.Items;
using System.Reflection;
using NBi.Xml.Settings;

namespace NBi.Testing.Unit.Xml
{
    public class InstanceSettlingXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.InstanceSettlingXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
                manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFilWithInstanceSettlinge_InstanceDefinitionNotNull()
        {
            TestSuiteXml ts = DeserializeSample();
            var test = ts.Tests[0] as TestXml;

            // Check the properties of the object.
            Assert.That(test.InstanceSettling, Is.Not.Null);
            Assert.That(test.InstanceSettling, Is.Not.EqualTo(InstanceSettlingXml.Unique));
        }


        [Test]
        public void Serialize_TestWithInstanceSettling_InstanceSettlingCorrectlySerialized()
        {
            var test = new TestXml()
            {
                InstanceSettling = new InstanceSettlingXml()
                {
                    Variable = new InstanceVariableXml() { Name = "firstOfMonth" }
                }
            };

            var serializer = new XmlSerializer(typeof(TestXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, test);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<instance-settling"));
            Assert.That(content, Is.StringContaining("<local-variable"));
            Assert.That(content, Is.Not.StringContaining("<category"));
            Assert.That(content, Is.Not.StringContaining("<trait"));
        }

        [Test]
        public void Deserialize_SampleFileWithoutInstanceSettling_InstanceDefinitionNotNull()
        {
            TestSuiteXml ts = DeserializeSample();
            var test = ts.Tests[1] as TestXml;

            // Check the properties of the object.
            Assert.That(test.InstanceSettling, Is.Not.Null);
            Assert.That(test.InstanceSettling, Is.EqualTo(InstanceSettlingXml.Unique));
        }

        [Test]
        public void Deserialize_SampleFile_InstanceDefinitionHasVariable()
        {
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[0].InstanceSettling.Variable, Is.Not.Null);
        }

        [Test]
        public void Serialize_TestWithoutInstanceSettling_InstanceSettlingNotSerialized()
        {
            var test = new TestXml();

            var serializer = new XmlSerializer(typeof(TestXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, test);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.Not.StringContaining("<instance-settling"));
        }

        [Test]
        public void Serialize_WithCategorieAndTrait_CategorieAndTraitNotSerialized()
        {
            var test = new TestXml()
            {
                InstanceSettling = new InstanceSettlingXml()
                {
                    Variable = new InstanceVariableXml() { Name = "firstOfMonth" },
                    Categories = new List<string>() { "~{@firstOfMonth:MMM}", "~{@firstOfMonth:MM}" },
                    Traits = new List<TraitXml>() { new TraitXml() { Name = "Year", Value = "~{@firstOfMonth:YYYY}" } }
                }
            };

            var serializer = new XmlSerializer(test.GetType());
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer.Serialize(writer, test);
                var content = Encoding.UTF8.GetString(stream.ToArray());


                Debug.WriteLine(content);

                Assert.That(content, Is.StringContaining("<instance-settling"));
                Assert.That(content, Is.StringContaining("<category"));
                Assert.That(content, Is.StringContaining("<trait"));
            }
        }
    }
}
