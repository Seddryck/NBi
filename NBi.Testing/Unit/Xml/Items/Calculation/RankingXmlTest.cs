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

namespace NBi.Testing.Unit.Xml.Items.Calculation
{
    public class RankingXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.RankingXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_RankingWithDefaultType_RankingXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking, Is.Not.Null);
            Assert.That(alteration.Filters[0].Ranking.Operand, Is.InstanceOf<IColumnIdentifier>());
            Assert.That(alteration.Filters[0].Ranking.Type, Is.EqualTo(ColumnType.Numeric));
        }

        [Test]
        public void Deserialize_RankingWithDefaultTop_RankingXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.Rank, Is.Not.Null);
            Assert.That(alteration.Filters[0].Ranking.Rank, Is.TypeOf<TopRankingXml>());
            Assert.That(alteration.Filters[0].Ranking.Rank.Count, Is.EqualTo(1));
        }

        [Test]
        public void Deserialize_RankingWithBottom_BottomRankingXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.Rank, Is.Not.Null);
            Assert.That(alteration.Filters[0].Ranking.Rank, Is.TypeOf<BottomRankingXml>());
            Assert.That(alteration.Filters[0].Ranking.Rank.Count, Is.EqualTo(3));
        }

        [Test]
        public void Deserialize_RankingWithGroupBy_GroupByXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.GroupBy, Is.Not.Null);
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_RankingWithColumn_ColumnDefinitionLightXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns[0].Identifier, Is.InstanceOf<IColumnIdentifier>());
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns[0].Type, Is.EqualTo(ColumnType.DateTime));
        }

        [Test]
        public void Deserialize_RankingWithDefaultColumn_ColumnDefinitionLightXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var alteration = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alteration;
            Assert.That(alteration.Filters, Is.Not.Null.And.Not.Empty);
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns[1].Identifier, Is.InstanceOf<IColumnIdentifier>());
            Assert.That(alteration.Filters[0].Ranking.GroupBy.Columns[1].Type, Is.EqualTo(ColumnType.Text));
        }

        [Test]
        public void Serialize_DefaultNoGroup_RankingXml()
        {
            var filterXml = new FilterXml
            {
                Ranking = new RankingXml()
                {
                    Operand = new ColumnPositionIdentifier(1),
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

            Assert.That(content, Is.StringContaining("ranking"));
            Assert.That(content, Is.StringContaining("top"));
            Assert.That(content, Is.Not.StringContaining("count"));
            Assert.That(content, Is.Not.StringContaining("type"));
        }
        [Test]
        public void Serialize_WithGroupBy_RankingXml()
        {
            var filterXml = new FilterXml
            {
                Ranking = new RankingXml()
                {
                    Operand = new ColumnPositionIdentifier(1),
                    Type = ColumnType.DateTime,
                    Rank = new BottomRankingXml()
                    {
                        Count = 3
                    },
                    GroupBy = new GroupByXml()
                    {
                        Columns = new List<ColumnDefinitionLightXml>()
                        {
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
                        }
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

            Assert.That(content, Is.StringContaining("bottom"));
            Assert.That(content, Is.StringContaining("dateTime"));
            Assert.That(content, Is.StringContaining("count"));
            Assert.That(content, Is.StringContaining("group-by"));
            Assert.That(content, Is.StringContaining("column"));
            Assert.That(content, Is.StringContaining("foo"));
            Assert.That(content, Is.StringContaining("boolean"));
            Assert.That(content, Is.StringContaining("bar"));
            Assert.That(content, Is.Not.StringContaining("text"));
        }
    }
}
