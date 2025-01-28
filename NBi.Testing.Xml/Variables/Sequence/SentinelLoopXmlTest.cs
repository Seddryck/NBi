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

namespace NBi.Xml.Testing.Unit.Variables.Sequence;

public class SentinelLoopXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_VariableHasSentinelLoop()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable as InstanceVariableXml;

        // Check the properties of the object.
        Assert.That(variable.SentinelLoop, Is.Not.Null);
        Assert.That(variable.SentinelLoop, Is.TypeOf<SentinelLoopXml>());
    }

    [Test]
    public void Deserialize_SampleFile_VariableHasCorrectNameAndType()
    {
        var ts = DeserializeSample();
        var variable = ts.Tests[0].InstanceSettling.Variable;

        // Check the properties of the object.
        Assert.That(variable.SentinelLoop!.Seed, Is.EqualTo("2016-01-01"));
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

        Assert.That(content, Does.Contain("<loop-sentinel"));
        Assert.That(content, Does.Contain("seed=\"1\""));
        Assert.That(content, Does.Contain("terminal=\"10\""));
        Assert.That(content, Does.Contain("step=\"2\""));
    }
}
