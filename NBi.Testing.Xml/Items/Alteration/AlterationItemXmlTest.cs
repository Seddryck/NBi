using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Items.Alteration;
using NBi.Xml.Items.Alteration.Mutation;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Xml.Items.Alteration
{
    public class AlterationItemXmlTest
    {
        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.AlterationXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_Filter_FilterColumnXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Mutations, Is.Not.Null.And.Not.Empty);
            Assert.That(rs.Alteration.Mutations.Count(), Is.EqualTo(1));
            Assert.That(rs.Alteration.Mutations[0], Is.TypeOf<HoldColumnXml>());
        }

        [Test]
        public void Deserialize_Skip_SkipColumnXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            Assert.That(rs.Alteration, Is.Not.Null);
            Assert.That(rs.Alteration.Mutations, Is.Not.Null.And.Not.Empty);
            Assert.That(rs.Alteration.Mutations.Count(), Is.EqualTo(1));
            Assert.That(rs.Alteration.Mutations[0], Is.TypeOf<RemoveColumnXml>());
        }

        [Test]
        public void Deserialize_FilterItems_Identifiers()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            var filter = rs.Alteration.Mutations[0] as HoldColumnXml;
            Assert.That(filter.Columns.Count(), Is.EqualTo(2));
            Assert.That(filter.Columns[0].Identifier, Is.EqualTo(new ColumnPositionIdentifier(0)));
            Assert.That(filter.Columns[1].Identifier, Is.EqualTo(new ColumnNameIdentifier("Foo")));
        }

        [Test]
        public void Deserialize_SkipItems_Identifiers()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            var skip = rs.Alteration.Mutations[0] as RemoveColumnXml;
            Assert.That(skip.Columns.Count(), Is.EqualTo(2));
            Assert.That(skip.Columns[0].Identifier, Is.EqualTo(new ColumnPositionIdentifier(0)));
            Assert.That(skip.Columns[1].Identifier, Is.EqualTo(new ColumnNameIdentifier("Foo")));
        }

        [Test]
        public void Serialize_FilterItems_ValidXml()
        {
            var alteration = new AlterationXml();
            alteration.Mutations.Add(new HoldColumnXml());
            (alteration.Mutations[0] as FilteringColumnXml).Columns.Add(new ColumnDefinitionLightXml() { Identifier = new ColumnPositionIdentifier(1) });

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<AlterationXml>(alteration);

            Assert.That(xml, Is.StringContaining("<hold"));
            Assert.That(xml, Is.StringContaining("<column"));
            Assert.That(xml, Is.StringContaining("identifier=\"#1\""));
        }


        [Test]
        public void Serialize_SkipItems_ValidXml()
        {
            var alteration = new AlterationXml();
            alteration.Mutations.Add(new RemoveColumnXml());
            (alteration.Mutations[0] as FilteringColumnXml).Columns.Add(new ColumnDefinitionLightXml() { Identifier = new ColumnNameIdentifier("Foo") });

            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<AlterationXml>(alteration);

            Assert.That(xml, Is.StringContaining("<remove"));
            Assert.That(xml, Is.StringContaining("<column"));
            Assert.That(xml, Is.StringContaining("identifier=\"[Foo]\""));
        }
    }
}
