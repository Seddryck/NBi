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
public class LookupMatchesXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyReferenceExists()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<LookupMatchesXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyJoinMapping()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var lookupMatches = (LookupMatchesXml)ts.Tests[testNr].Constraints[0]!;
        var mappings = lookupMatches.Join!.Mappings;

        Assert.That(mappings, Has.Count.EqualTo(1));
        Assert.That(mappings[0].Candidate, Is.EqualTo("DepartmentID"));
        Assert.That(mappings[0].Reference, Is.EqualTo("Id"));
        Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Numeric));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyInclusionMapping()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var lookupMatches = (LookupMatchesXml)ts.Tests[testNr].Constraints[0]!;
        var mappings = lookupMatches.Inclusion!.Mappings;
         
        Assert.That(mappings, Has.Count.EqualTo(1));
        Assert.That(mappings[0].Candidate, Is.EqualTo("DepartmentName"));
        Assert.That(mappings[0].Reference, Is.EqualTo("Name"));
        Assert.That(mappings[0].Type, Is.EqualTo(ColumnType.Text));
    }
}
