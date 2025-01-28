using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class ContainXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ContainCaptionNotIgnoringCaseImplicitely()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<ContainXml>());
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(1));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0].ToLower(), Is.EqualTo("xyz"));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ContainCaptionIgnoringCaseExplicitely()
    {
        int testNr = 1;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(1));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0].ToLower(), Is.EqualTo("xyz"));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).IgnoreCase, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_ContainReadItems()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items, Has.Count.EqualTo(2));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[0], Is.EqualTo("xyz"));
        Assert.That(((ContainXml)ts.Tests[testNr].Constraints[0]).Items[1], Is.EqualTo("abc"));
    }


    [Test]
    public void Serialize_ContainWithCaption_ContainItems()
    {
        var containXml = new ContainXml
        {
            Items = ["myMember"]
        };

        var serializer = new XmlSerializer(typeof(ContainXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, containXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Not.Contain("caption"));
        Assert.That(content, Does.Contain("<item>"));
        Assert.That(content, Does.Contain("myMember"));
    }
}
