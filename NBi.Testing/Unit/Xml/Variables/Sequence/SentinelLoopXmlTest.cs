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
using NBi.Xml.Variables.Sequence;

namespace NBi.Testing.Unit.Xml.Variables.Sequence
{
    public class SentinelLoopXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.SentinelLoopXmlTestSuite.xml"))
                using (StreamReader reader = new StreamReader(stream))
                    manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_VariableHasSentinelLoop()
        {
            TestSuiteXml ts = DeserializeSample();
            var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

            // Check the properties of the object.
            Assert.That(variable.SentinelLoop, Is.Not.Null);
            Assert.That(variable.SentinelLoop, Is.TypeOf<SentinelLoopXml>());
        }

        [Test]
        public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
        {
            TestSuiteXml ts = DeserializeSample();
            var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

            // Check the properties of the object.
            Assert.That(variable.SentinelLoop.Seed, Is.EqualTo("2016-01-01"));
            Assert.That(variable.SentinelLoop.Terminal, Is.EqualTo("2016-12-01"));
            Assert.That(variable.SentinelLoop.Step, Is.EqualTo("1 month"));
        }

        [Test]
        public void Serialize_Variable_SentinelLoopCorrectlySerialized()
        {
            var instanceVariable = new InstanceVariableXml()
            {
                SentinelLoop = new SentinelLoopXml()
                {
                    Seed = "1",
                    Terminal= "10",
                    Step = "2",
                }
            };

            var serializer = new XmlSerializer(typeof(InstanceVariableXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, instanceVariable);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<loop-sentinel"));
            Assert.That(content, Is.StringContaining("seed=\"1\""));
            Assert.That(content, Is.StringContaining("terminal=\"10\""));
            Assert.That(content, Is.StringContaining("step=\"2\""));
        }
    }
}
