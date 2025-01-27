using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class LookupExistsXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyReferenceExists()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<LookupExistsXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyMapping()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var lookupExists = (LookupExistsXml)ts.Tests[testNr].Constraints[0]!;
        var mappings = lookupExists.Join!.Mappings;

        Assert.That(mappings, Has.Count.EqualTo(1));
        Assert.That(mappings[0].Candidate, Is.EqualTo("GroupId"));
        Assert.That(mappings[0].Reference, Is.EqualTo("Id"));
        Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Numeric));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyReverseWhenMissing()
    {
        int testNr = 0;

        var ts = DeserializeSample();
        var lookupExists = (LookupExistsXml)ts.Tests[testNr].Constraints[0]!;
        var isReversed = lookupExists.IsReversed;

        Assert.That(isReversed, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyMappings()
    {
        int testNr = 1;

        var ts = DeserializeSample();
        var lookupExists = (LookupExistsXml)ts.Tests[testNr].Constraints[0]!;
        var mappings = lookupExists.Join!.Mappings;

        Assert.That(mappings, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyReverse()
    {
        int testNr = 2;

        var ts = DeserializeSample();
        var lookupExists = (LookupExistsXml)ts.Tests[testNr].Constraints[0]!;
        var isReversed = lookupExists.IsReversed;

        Assert.That(isReversed, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyUsing()
    {
        int testNr = 3;

        var ts = DeserializeSample();
        var lookupExists = (LookupExistsXml)ts.Tests[testNr].Constraints[0]!;
        var usings = lookupExists.Join!.Usings;

        Assert.That(usings, Has.Count.EqualTo(1));
        Assert.That(usings[0].Column, Is.EqualTo("#0"));
    }

    [Test]
    public void Serialize_ReferenceExistsXml_Correct()
    {
        var lookupExistsXml = new LookupExistsXml()
        {
            Join = new JoinXml()
            {
                Mappings =
                [
                    new ColumnMappingXml() {Candidate = "#1", Reference="Col1", Type=ColumnType.Numeric},
                    new ColumnMappingXml() {Candidate = "#0", Reference="Col2", Type=ColumnType.Text}
                ]
            },
            ResultSet = new ResultSetSystemXml()
        };

        var serializer = new XmlSerializer(typeof(LookupExistsXml));
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, Encoding.UTF8);
        serializer.Serialize(writer, lookupExistsXml);
        var content = Encoding.UTF8.GetString(stream.ToArray());
        writer.Close();
        stream.Close();

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("mapping"));
        Assert.That(content, Does.Contain("reference"));
        Assert.That(content, Does.Contain("candidate"));
        Assert.That(content, Does.Contain("numeric"));
        Assert.That(content, Does.Not.Contain("reverse"));
    }
}
