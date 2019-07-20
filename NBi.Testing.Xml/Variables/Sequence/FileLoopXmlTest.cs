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

namespace NBi.Testing.Xml.Unit.Variables.Sequence
{
    public class FileLoopXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.FileLoopXmlTestSuite.xml"))
                using (StreamReader reader = new StreamReader(stream))
                    manager.Read(reader);

            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_VariableHasFileLoop()
        {
            TestSuiteXml ts = DeserializeSample();
            var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

            // Check the properties of the object.
            Assert.That(variable.FileLoop, Is.Not.Null);
            Assert.That(variable.FileLoop, Is.TypeOf<FileLoopXml>());
        }

        [Test]
        public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
        {
            TestSuiteXml ts = DeserializeSample();
            var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

            // Check the properties of the object.
            Assert.That(variable.FileLoop.Path, Is.EqualTo(@"C:\Temp\"));
            Assert.That(variable.FileLoop.Pattern, Is.EqualTo("foo-*.txt"));
        }

        [Test]
        public void Serialize_Variable_FileLoopCorrectlySerialized()
        {
            var instanceVariable = new InstanceVariableXml()
            {
                FileLoop = new FileLoopXml()
                {
                    Path = @"C:\Temp\",
                    Pattern = "foo-*.txt",
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

            Assert.That(content, Does.Contain("<loop-file"));
            Assert.That(content, Does.Contain("path=\"C:\\Temp\\\""));
            Assert.That(content, Does.Contain("pattern=\"foo-*.txt\""));
        }
    }
}
