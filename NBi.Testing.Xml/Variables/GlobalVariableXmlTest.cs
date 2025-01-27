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
using NBi.Xml.Variables.Custom;

namespace NBi.Xml.Testing.Unit.Variables;

public class GlobalVariableXmlTest
{
    [Test]
    public void Serialize_OneCSharp_Correct()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables =
            [
                new GlobalVariableXml()
                {
                    Name="myCSharp",
                    Script = new ScriptXml() {Language=NBi.Core.Transformation.LanguageType.CSharp, Code="0+0" }
                }
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<variables"));
        Assert.That(content, Does.Contain("<variable name=\"myCSharp\""));
        Assert.That(content, Does.Contain("0+0"));
    }

    [Test]
    public void Serialize_OneQuery_Correct()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables =
            [
                new GlobalVariableXml()
                {
                    Name="mySQL",
                    QueryScalar = new QueryScalarXml() {ConnectionString= "myConnString", InlineQuery="select * from myTable;"}
                }
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<variables"));
        Assert.That(content, Does.Contain("<variable name=\"mySQL\""));
        Assert.That(content, Does.Contain("select * from myTable;"));
    }

    [Test]
    public void Serialize_OneEnvironment_Correct()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables =
            [
                new GlobalVariableXml()
                {
                    Name="myVar",
                    Environment = new EnvironmentXml() {Name="myEnvVar"}
                }
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<variables"));
        Assert.That(content, Does.Contain("<variable name=\"myVar\""));
        Assert.That(content, Does.Contain("<environment name=\"myEnvVar\""));
    }

    [Test]
    public void Serialize_TwoVariables_Correct()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables =
            [
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
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<variables"));
        Assert.That(content, Does.Contain("<variable name=\"mySQL\""));
        Assert.That(content, Does.Contain("<variable name=\"myCSharp\""));
    }

    [Test]
    public void Serialize_NoVariable_NothingSerialized()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables = []
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Not.Contain("<variables"));
        Assert.That(content, Does.Not.Contain("<variable"));
    }

    [Test]
    public void Serialize_CustomEvaluation_CustomElement()
    {
        var testSuiteXml = new TestSuiteXml()
        {
            Variables =
            [
                new GlobalVariableXml
                {
                    Name= "myVar",
                    Custom = new CustomXml
                    {
                        AssemblyPath = "AssemblyPath\\myAssembly.dll",
                        TypeName = "@VarType",
                        Parameters =
                        [
                            new CustomParameterXml{Name="myParam", StringValue="@VarParam"}
                        ]
                    }
                }
            ]
        };

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, testSuiteXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<variable name=\"myVar\""));
            Assert.That(content, Does.Contain("<custom"));
            Assert.That(content, Does.Contain("assembly-path=\"AssemblyPath\\myAssembly.dll\""));
            Assert.That(content, Does.Contain("type=\"@VarType\""));
            Assert.That(content, Does.Contain("<parameter name=\"myParam\">"));
            Assert.That(content, Does.Contain("@VarParam"));
        }
    }
}
