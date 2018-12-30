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

namespace NBi.Testing.Unit.Xml.Variables
{
    public class GlobalVariableXmlTest
    {
        [Test]
        public void Serialize_OneCSharp_Correct()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Variables = new List<GlobalVariableXml>()
                {
                    new GlobalVariableXml()
                    {
                        Name="myCSharp",
                        Script = new ScriptXml() {Language=NBi.Core.Transformation.LanguageType.CSharp, Code="0+0" }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TestSuiteXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<variables"));
            Assert.That(content, Is.StringContaining("<variable name=\"myCSharp\""));
            Assert.That(content, Is.StringContaining("0+0"));
        }

        [Test]
        public void Serialize_OneQuery_Correct()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Variables = new List<GlobalVariableXml>()
                {
                    new GlobalVariableXml()
                    {
                        Name="mySQL",
                        QueryScalar = new QueryScalarXml() {ConnectionString= "myConnString", InlineQuery="select * from myTable;"}
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TestSuiteXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<variables"));
            Assert.That(content, Is.StringContaining("<variable name=\"mySQL\""));
            Assert.That(content, Is.StringContaining("select * from myTable;"));
        }

        [Test]
        public void Serialize_OneEnvironment_Correct()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Variables = new List<GlobalVariableXml>()
                {
                    new GlobalVariableXml()
                    {
                        Name="myVar",
                        Environment = new EnvironmentXml() {Name="myEnvVar"}
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TestSuiteXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<variables"));
            Assert.That(content, Is.StringContaining("<variable name=\"myVar\""));
            Assert.That(content, Is.StringContaining("<environment name=\"myEnvVar\""));
        }

        [Test]
        public void Serialize_TwoVariables_Correct()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Variables = new List<GlobalVariableXml>()
                {
                    new GlobalVariableXml()
                    {
                        Name="mySQL",
                        QueryScalar = new QueryScalarXml() {ConnectionString= "myConnString", InlineQuery="select * from myTable;"}
                    },
                    new GlobalVariableXml()
                    {
                        Name="myCSharp",
                        Script = new ScriptXml() {Language=NBi.Core.Transformation.LanguageType.CSharp, Code="0+0" }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TestSuiteXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<variables"));
            Assert.That(content, Is.StringContaining("<variable name=\"mySQL\""));
            Assert.That(content, Is.StringContaining("<variable name=\"myCSharp\""));
        }

        [Test]
        public void Serialize_NoVariable_NothingSerialized()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Variables = new List<GlobalVariableXml>()
            };

            var serializer = new XmlSerializer(typeof(TestSuiteXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.Not.StringContaining("<variables"));
            Assert.That(content, Is.Not.StringContaining("<variable"));
        }
    }
}
