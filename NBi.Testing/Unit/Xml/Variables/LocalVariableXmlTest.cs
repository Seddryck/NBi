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
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, instanceSetting);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<local-variable"));
            Assert.That(content, Is.StringContaining("name=\"firstOfMonth\""));
            Assert.That(content, Is.StringContaining("type=\"dateTime\""));
        }
    }
}
