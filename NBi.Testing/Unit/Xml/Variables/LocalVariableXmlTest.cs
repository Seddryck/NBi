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
using NBi.Core.ResultSet;

namespace NBi.Testing.Unit.Xml.Variables
{
    public class LocalVariableXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.LocalVariableXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
                manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_InstanceDefinitionHasVariable()
        {
            TestSuiteXml ts = DeserializeSample();
            var instance = ts.Tests[0].InstanceSettling as InstanceSettlingXml;

            // Check the properties of the object.
            Assert.That(instance.Variable, Is.Not.Null);
        }

        [Test]
        public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
        {
            TestSuiteXml ts = DeserializeSample();
            var instance = ts.Tests[0].InstanceSettling as InstanceSettlingXml;

            // Check the properties of the object.
            Assert.That(instance.Variable.Name, Is.EqualTo("firstDayOfMonth"));
            Assert.That(instance.Variable.Type, Is.EqualTo(ColumnType.DateTime));
        }

        [Test]
        public void Serialize_InstanceSetting_LocalVariableCorrectlySerialized()
        {
            var instanceSetting = new InstanceSettlingXml()
            {
                Variable = new InstanceVariableXml()
                {
                    Name = "firstOfMonth",
                    Type = ColumnType.DateTime,
                }
            };

            var serializer = new XmlSerializer(typeof(InstanceSettlingXml));
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer.Serialize(writer, instanceSetting);
                var content = Encoding.UTF8.GetString(stream.ToArray());

                Debug.WriteLine(content);

                Assert.That(content, Is.StringContaining("<local-variable"));
                Assert.That(content, Is.StringContaining("name=\"firstOfMonth\""));
                Assert.That(content, Is.StringContaining("type=\"dateTime\""));
                Assert.That(content, Is.Not.StringContaining("<item"));
            }
        }

        [Test]
        public void Deserialize_Items_ListOfItems()
        {
            TestSuiteXml ts = DeserializeSample();
            var variable = ts.Tests[1].InstanceSettling.Variable as InstanceVariableXml;

            // Check the properties of the object.
            Assert.That(variable.Items, Is.Not.Null);
            Assert.That(variable.Items, Has.Count.EqualTo(4));
            Assert.That(variable.Items, Has.Member("Spring"));
            Assert.That(variable.Items, Has.Member("Summer"));
            Assert.That(variable.Items, Has.Member("Fall"));
            Assert.That(variable.Items, Has.Member("Winter"));
        }

        [Test]
        public void Serialize_Items_ItemCorrectlySerialized()
        {
            var root = new InstanceSettlingXml()
            {
                Variable = new InstanceVariableXml()
                {
                    Name = "season",
                    Type = ColumnType.Text,
                    Items = new List<string>() { "Spring", "Summer", "Fall", "Winter" }
                }
            };

            var serializer = new XmlSerializer(root.GetType());
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                serializer.Serialize(writer, root);
                var content = Encoding.UTF8.GetString(stream.ToArray());

                Debug.WriteLine(content);

                Assert.That(content, Is.StringContaining("<local-variable"));
                Assert.That(content, Is.StringContaining("name=\"season\""));
                Assert.That(content, Is.StringContaining("type=\"text\""));
                Assert.That(content, Is.Not.StringContaining("loop"));
                Assert.That(content, Is.StringContaining("<item"));
                Assert.That(content, Is.StringContaining(">Spring<"));
                Assert.That(content, Is.StringContaining(">Summer<"));
                Assert.That(content, Is.StringContaining(">Fall<"));
                Assert.That(content, Is.StringContaining(">Winter<"));
            }
        }
    }
}
