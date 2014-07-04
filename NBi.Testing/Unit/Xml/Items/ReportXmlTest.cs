using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class ReportXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.ReportXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_ReportWithEverythingDefined_ReportXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
            var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(report, Is.Not.Null);
            Assert.That(report.Source, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=ReportServer;Integrated Security=True;"));
            Assert.That(report.Path, Is.EqualTo("/AdventureWorks Sample Reports/"));
            Assert.That(report.Name, Is.EqualTo("Currency_List"));
            Assert.That(report.Dataset, Is.EqualTo("currency"));
            Assert.That(report.GetConnectionString(), Is.EqualTo(@"Data Source=tadam;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_ReportWithoutConnectionString_ReportXmlUsingDefaultConnectionString()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
            var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(report.GetConnectionString(), Is.EqualTo(@"Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_ReportWithTwoParameters_ReportXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<ReportXml>());
            var report = (ReportXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(report.Parameters, Is.Not.Null);
            Assert.That(report.Parameters, Has.Count.EqualTo(2));
        }
    }
}
