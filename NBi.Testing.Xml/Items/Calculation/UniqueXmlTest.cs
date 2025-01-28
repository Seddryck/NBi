using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.Calculation.Grouping;
using NBi.Xml.Items.Calculation.Ranking;
using NBi.Xml.Items.ResultSet;
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

namespace NBi.Xml.Testing.Unit.Items.Calculation;

public class UniqueXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_RankingWithDefaultType_RankingXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Uniqueness, Is.Not.Null);
    }

    [Test]
    public void Deserialize_UniqueWithGroupBy_GroupByXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Uniqueness.GroupBy, Is.Not.Null);
        Assert.That(((FilterXml)alterations[0]).Uniqueness.GroupBy.Columns, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Uniqueness.GroupBy.Columns, Has.Count.EqualTo(2));
    }
}
