using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    class LogXmlTest
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
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.LogXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SystemUnderTestConditionContentFileSpecified_CorrectlyAssigned()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs, Is.Not.Null.And.Not.Empty);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs, Has.Count.EqualTo(2));
            
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[0], Is.TypeOf<LogXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[0], Is.Not.Null);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[0].Condition, Is.EqualTo(LogXml.LogCondition.Failure));
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[0].Content, Is.EqualTo(LogXml.LogContent.ResultSet));
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[0].File, Is.EqualTo("myFilename"));

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[1], Is.TypeOf<LogXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[1], Is.Not.Null);
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[1].Condition, Is.EqualTo(LogXml.LogCondition.Always));
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[1].Content, Is.EqualTo(LogXml.LogContent.Statistics));
        }

        [Test]
        [Ignore]
        public void Deserialize_FileNotSpecified_CorrectlyAssigned()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).Logs[1].File, Is.EqualTo(ts.Tests[testNr].Name));
        }


        [Test]
        public void Deserialize_AssertConditionContentFileSpecified_CorrectlyAssigned()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs, Is.Not.Null.And.Not.Empty);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs, Has.Count.EqualTo(1));

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs[0], Is.TypeOf<LogXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs[0], Is.Not.Null);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs[0].Condition, Is.EqualTo(LogXml.LogCondition.Failure));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs[0].Content, Is.EqualTo(LogXml.LogContent.ResultSet));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Logs[0].File, Is.EqualTo("myOtherFilename"));
        }
    }
}
