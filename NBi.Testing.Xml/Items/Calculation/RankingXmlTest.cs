using NBi.Core.ResultSet;
using NBi.Extensibility;
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

public class RankingXmlTest : BaseXmlTest
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
        Assert.That(((FilterXml)alterations[0]).Ranking, Is.Not.Null);
        Assert.That(((FilterXml)alterations[0]).Ranking.Operand, Is.InstanceOf<IColumnIdentifier>());
        Assert.That(((FilterXml)alterations[0]).Ranking.Type, Is.EqualTo(ColumnType.Numeric));
    }

    [Test]
    public void Deserialize_RankingWithDefaultTop_RankingXml()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank, Is.Not.Null);
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank, Is.TypeOf<TopRankingXml>());
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank.Count, Is.EqualTo(1));
    }

    [Test]
    public void Deserialize_RankingWithBottom_BottomRankingXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank, Is.Not.Null);
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank, Is.TypeOf<BottomRankingXml>());
        Assert.That(((FilterXml)alterations[0]).Ranking.Rank.Count, Is.EqualTo(3));
    }

    [Test]
    public void Deserialize_RankingWithGroupBy_GroupByXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy, Is.Not.Null);
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_RankingWithColumn_ColumnDefinitionLightXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns[0].Identifier, Is.InstanceOf<IColumnIdentifier>());
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns[0].Type, Is.EqualTo(ColumnType.DateTime));
    }

    [Test]
    public void Deserialize_RankingWithDefaultColumn_ColumnDefinitionLightXml()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        Assert.That(alterations, Is.Not.Null.And.Not.Empty);
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns[1].Identifier, Is.InstanceOf<IColumnIdentifier>());
        Assert.That(((FilterXml)alterations[0]).Ranking.GroupBy.Columns[1].Type, Is.EqualTo(ColumnType.Text));
    }

    [Test]
    public void Serialize_DefaultNoGroup_RankingXml()
    {
        var filterXml = new FilterXml
        {
            Ranking = new RankingXml()
            {
                Operand = new ColumnOrdinalIdentifier(1),
                Rank = new TopRankingXml()
            }
        };

        var serializer = new XmlSerializer(typeof(FilterXml));
        var content = string.Empty;
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, filterXml);
            content = Encoding.UTF8.GetString(stream.ToArray());
        }

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("ranking"));
        Assert.That(content, Does.Contain("top"));
        Assert.That(content, Does.Not.Contain("count"));
        Assert.That(content, Does.Not.Contain("type"));
    }
    [Test]
    public void Serialize_WithGroupBy_RankingXml()
    {
        var filterXml = new FilterXml
        {
            Ranking = new RankingXml()
            {
                Operand = new ColumnOrdinalIdentifier(1),
                Type = ColumnType.DateTime,
                Rank = new BottomRankingXml()
                {
                    Count = 3
                },
                GroupBy = new GroupByXml()
                {
                    Columns =
                    [
                        new ColumnDefinitionLightXml()
                        {
                            Identifier =new ColumnNameIdentifier("foo"),
                            Type =ColumnType.Boolean
                        },
                        new ColumnDefinitionLightXml()
                        {
                            Identifier =new ColumnNameIdentifier("bar"),
                            Type = ColumnType.Text
                        },
                    ]
                }
            }
        };

        var serializer = new XmlSerializer(typeof(FilterXml));
        var content = string.Empty;
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream, Encoding.UTF8))
        {
            serializer.Serialize(writer, filterXml);
            content = Encoding.UTF8.GetString(stream.ToArray());
        }

        Debug.WriteLine(content);

        Assert.That(content, Does.Contain("bottom"));
        Assert.That(content, Does.Contain("dateTime"));
        Assert.That(content, Does.Contain("count"));
        Assert.That(content, Does.Contain("group-by"));
        Assert.That(content, Does.Contain("column"));
        Assert.That(content, Does.Contain("foo"));
        Assert.That(content, Does.Contain("boolean"));
        Assert.That(content, Does.Contain("bar"));
        Assert.That(content, Does.Not.Contain("text"));
    }

    [Test]
    public void Deserialize_RankingWithCases_CaseGrouping()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        var filter = alterations[0] as FilterXml;
        Assert.That(filter, Is.Not.Null);
        Assert.That(filter!.Ranking.GroupBy, Is.Not.Null);
        Assert.That(filter.Ranking.GroupBy.Cases, Is.Not.Null);
        Assert.That(filter.Ranking.GroupBy.Cases.Count, Is.EqualTo(2));
    }

    [Test]
    public void Deserialize_RankingWithCases_PredicateOrCombination()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
        var alterations = ((ResultSetSystemXml)ts.Tests[testNr].Systems[0]).Alterations;
        var filter = alterations[0] as FilterXml;
        Assert.That(filter, Is.Not.Null);
        Assert.That(filter!.Ranking.GroupBy.Cases[0].Predication, Is.Not.Null);
        Assert.That(filter.Ranking.GroupBy.Cases[0].Predication, Is.TypeOf<SinglePredicationXml>());
        Assert.That(filter.Ranking.GroupBy.Cases[1].Predication, Is.Not.Null);
        Assert.That(filter.Ranking.GroupBy.Cases[1].Predication, Is.TypeOf<CombinationPredicationXml>());
    }

}
