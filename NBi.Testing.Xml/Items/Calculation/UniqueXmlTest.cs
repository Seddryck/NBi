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

namespace NBi.Testing.Xml.Unit.Items.Calculation
{
    public class UniqueXmlTest : BaseXmlTest
    {

        [Test]
        public void Deserialize_RankingWithDefaultType_RankingXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var alterations = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alterations;
            Assert.That(alterations, Is.Not.Null.And.Not.Empty);
            Assert.That((alterations[0] as FilterXml).Uniqueness, Is.Not.Null);
        }

        [Test]
        public void Deserialize_UniqueWithGroupBy_GroupByXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            var ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var alterations = (ts.Tests[testNr].Systems[0] as ResultSetSystemXml).Alterations;
            Assert.That(alterations, Is.Not.Null.And.Not.Empty);
            Assert.That((alterations[0] as FilterXml).Uniqueness.GroupBy, Is.Not.Null);
            Assert.That((alterations[0] as FilterXml).Uniqueness.GroupBy.Columns, Is.Not.Null.And.Not.Empty);
            Assert.That((alterations[0] as FilterXml).Uniqueness.GroupBy.Columns, Has.Count.EqualTo(2));
        }
    }
}
