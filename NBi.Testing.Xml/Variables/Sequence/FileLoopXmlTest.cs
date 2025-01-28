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
using NBi.Core.Calculation;

namespace NBi.Xml.Testing.Unit.Variables.Sequence;

public class FileLoopXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_VariableHasFileLoop()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

        // Check the properties of the object.
        Assert.That(variable.FileLoop, Is.Not.Null);
        Assert.That(variable.FileLoop, Is.TypeOf<FileLoopXml>());
    }

    [Test]
    public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable;

        // Check the properties of the object.
        Assert.That(variable.FileLoop!.Path, Is.EqualTo(@"C:\Temp\"));
        Assert.That(variable.FileLoop.Pattern, Is.EqualTo("foo-*.txt"));
    }


    [Test]
    public void Deserialize_SampleFile_FilterCorrectly()
    {
        var ts = DeserializeSample();
        var localVariable = ts.Tests[1].InstanceSettling.Variable;

        // Check the properties of the object.
        Assert.That(localVariable.Filter!.Predication.Operand, Is.EqualTo(@"value | file-to-size(C:\Temp\)"));
        Assert.That(localVariable.Filter.Predication.Predicate!.ComparerType, Is.EqualTo(ComparerType.MoreThan));
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
