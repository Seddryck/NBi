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
    public class SharedDatasetXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.SharedDatasetXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SharedDatasetWithEverythingDefined_SharedDatasetXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset, Is.Not.Null);
            Assert.That(sharedDataset.Source, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=ReportServer;Integrated Security=True;"));
            Assert.That(sharedDataset.Path, Is.EqualTo("/AdventureWorks Sample Reports/"));
            Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
            Assert.That(sharedDataset.GetConnectionString(), Is.EqualTo(@"Data Source=tadam;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_SharedDatasetWithoutConnectionString_SharedDatasetXmlUsingDefaultConnectionString()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset.GetConnectionString(), Is.EqualTo(@"Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_SharedDatasetWithTwoParameters_SharedDatasetXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset.Parameters, Is.Not.Null);
            Assert.That(sharedDataset.Parameters, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_SharedDatasetWithReference_SharedDatasetXml()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset, Is.Not.Null);
            Assert.That(sharedDataset.Source, Is.EqualTo(@"http://reports.com/reports"));
            Assert.That(sharedDataset.Path, Is.EqualTo("Dashboard"));
            Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
            Assert.That(sharedDataset.GetConnectionString(), Is.EqualTo(@"Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_SharedDatasetWithDefault_SharedDatasetXml()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset, Is.Not.Null);
            Assert.That(sharedDataset.Source, Is.EqualTo(@"http://new.reports.com/reports"));
            Assert.That(sharedDataset.Path, Is.EqualTo("Details"));
            Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
            Assert.That(sharedDataset.GetConnectionString(), Is.EqualTo(@"Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }

        [Test]
        public void Deserialize_SharedDatasetWithMixDefaultReference_SharedDatasetXml()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<SharedDatasetXml>());
            var sharedDataset = (SharedDatasetXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(sharedDataset, Is.Not.Null);
            Assert.That(sharedDataset.Source, Is.EqualTo(@"http://new.reports.com/reports"));
            Assert.That(sharedDataset.Path, Is.EqualTo("alternate"));
            Assert.That(sharedDataset.Name, Is.EqualTo("EmpSalesMonth"));
            Assert.That(sharedDataset.GetConnectionString(), Is.EqualTo(@"Data Source=mhknbn2kdz.database.windows.net;Initial Catalog=AdventureWorks2012;User Id=sqlfamily;password=sqlf@m1ly"));
        }
    }
}
