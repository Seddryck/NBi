using System;
using NBi.Xml;
using NUnit.Framework;
using NBi.Core.Transformation;
using NBi.Xml.Variables;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace NBi.Xml.Testing.Unit;

[TestFixture]
public class TestSuiteWithVariablesTest
{
    [Test]
    public void Load_ValidFile_Success()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestSuite.xml", $"{GetType().Assembly.GetName().Name}.Resources.TestSuiteWithVariablesTestSuite.xml");

        var manager = new XmlManager();
        manager.Load(filename);

        Assert.That(manager.TestSuite, Is.Not.Null);
        Assert.That(manager.TestSuite!.Tests, Has.Count.EqualTo(1));
    }

    [Test]
    public void Load_ValidFile_VariablesLoaded()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestContentIsCorrect.xml", $"{GetType().Assembly.GetName().Name}.Resources.TestSuiteWithVariablesTestSuite.xml");

        var manager = new XmlManager();
        manager.Load(filename);

        Assert.That(manager.TestSuite!.Variables, Has.Count.EqualTo(2));
        Assert.That(manager.TestSuite.Variables[0].Name, Is.EqualTo("year"));
        Assert.That(manager.TestSuite.Variables[0].Script.Language, Is.EqualTo(LanguageType.CSharp));
        Assert.That(manager.TestSuite.Variables[0].Script.Code, Is.EqualTo("DateTime.Now.Year"));
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public void Load_ValidFileImplicitLanguage_LanguageSetToCSharp()
    {
        var filename = FileOnDisk.CreatePhysicalFile("TestContentIsCorrect.xml", $"{GetType().Assembly.GetName().Name}.Resources.TestSuiteWithVariablesTestSuite.xml");

        var manager = new XmlManager();
        manager.Load(filename);

        Assert.That(manager.TestSuite!.Variables[1].Script.Language, Is.EqualTo(LanguageType.CSharp));
    }

    [Test]
    public void Serialize_TestSuiteWithVariables_Correct()
    {
        var testSuiteXml = new TestSuiteXml();
        testSuiteXml.Variables.Add(new GlobalVariableXml()
        {
            Name = "year",
            Script = new ScriptXml()
            {
                Language= LanguageType.CSharp,
                Code="DateTime.Now.Year"
            }
        });

        var serializer = new XmlSerializer(typeof(TestSuiteXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testSuiteXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<variables"));
        Assert.That(content, Does.Contain("<variable name=\"year\">"));
        Assert.That(content, Does.Contain("<script"));
        Assert.That(content, Does.Contain("DateTime.Now.Year"));
    }
}
