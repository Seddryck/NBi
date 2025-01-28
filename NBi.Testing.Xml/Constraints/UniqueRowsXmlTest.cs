#region Using directives
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using NBi.Xml.Items.ResultSet;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class UniqueRowsXmlTest : BaseXmlTest
{
    [Test]
    public void Deserialize_SampleFile_IsConstraint()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<UniqueRowsXml>());
    }
    
    [Test]
    public void Serialize_NoDuplicate_CorrectConstraint()
    {
        var noDuplicate = new UniqueRowsXml();

        var testXml = new TestXml();
        testXml.Constraints.Add(noDuplicate);

        var serializer = new XmlSerializer(typeof(TestXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("<unique-rows />"));
    }

    [Test]
    public void Serialize_UniqueRowsWithColumnName_CorrectConstraint()
    {
        var uniqueRows = new UniqueRowsXml()
        {
            Columns = [new ColumnDefinitionXml() { Name = "myName" }]

        };

        var testXml = new TestXml();
        testXml.Constraints.Add(uniqueRows);

        var serializer = new XmlSerializer(typeof(TestXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("name="));
        Assert.That(content, Does.Contain("myName"));
        Assert.That(content, Does.Not.Contain("index="));
    }

    [Test]
    public void Serialize_UniqueRowsWithColumnIndex_CorrectConstraint()
    {
        var uniqueRows = new UniqueRowsXml()
        {
            Columns = [new ColumnDefinitionXml() { Index = 0 }]
        };

        var testXml = new TestXml();
        testXml.Constraints.Add(uniqueRows);

        var serializer = new XmlSerializer(typeof(TestXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, testXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("index="));
        Assert.That(content, Does.Contain("0"));
        Assert.That(content, Does.Not.Contain("name="));
    }
}
