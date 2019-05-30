using NBi.Core.ResultSet.Combination;
using NBi.Xml;
using NBi.Xml.Items.ResultSet.Combination;
using NBi.Xml.Systems;
using NBi.Xml.Variables.Sequence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Xml.Items.ResultSet
{
    public class SequenceCombinationXmlTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        protected TestSuiteXml DeserializeSample()
        {
            var manager = new XmlManager();
            using (var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream(
                            $"{Assembly.GetExecutingAssembly().GetName().Name}.Unit.Xml.Resources.{GetType().Name}Suite.xml"))
            using (var reader = new StreamReader(stream))
                manager.Read(reader);
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SequenceCombination_TwoSequences()
        {
            int testNr = 0;
            var ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

            Assert.That(rs.SequenceCombination, Is.Not.Null);
            Assert.That(rs.SequenceCombination, Is.TypeOf<SequenceCombinationXml>());
            Assert.That(rs.SequenceCombination.Operation, Is.EqualTo(SequenceCombinationOperation.CartesianProduct));
            Assert.That(rs.SequenceCombination.Sequences, Is.Not.Null.And.Not.Empty);
            Assert.That(rs.SequenceCombination.Sequences, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_Sequence_TwoItems()
        {
            int testNr = 0;
            var ts = DeserializeSample();

            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            var sequence = rs.SequenceCombination.Sequences.First();
            Assert.That(sequence, Is.Not.Null);
            Assert.That(sequence, Is.TypeOf<SequenceXml>());
            Assert.That(sequence.Items, Is.Not.Null.And.Not.Empty);
            Assert.That(sequence.Items, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_Sequence_Sentinel()
        {
            int testNr = 0;
            var ts = DeserializeSample();

            var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;
            var sequence = rs.SequenceCombination.Sequences.Last();
            Assert.That(sequence, Is.Not.Null);
            Assert.That(sequence, Is.TypeOf<SequenceXml>());
            Assert.That(sequence.SentinelLoop, Is.Not.Null);
            Assert.That(sequence.SentinelLoop, Is.TypeOf<SentinelLoopXml>());
        }
    }
}
