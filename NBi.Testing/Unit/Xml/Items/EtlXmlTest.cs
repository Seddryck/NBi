using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core.Etl;
using NBi.Xml;
using NBi.Xml.Decoration.Command;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml.Items
{
    [TestFixture]
    public class EtlXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.EtlXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_EtlFromFileInSetup_EtlXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<IEtlRunCommand>());
            var etl = ts.Tests[testNr].Setup.Commands[0] as EtlRunXml;

            Assert.That(etl, Is.Not.Null);
            Assert.That(etl.Server, Is.Null.Or.Empty);
            Assert.That(etl.Path, Is.EqualTo("/Etl/"));
            Assert.That(etl.Name, Is.EqualTo("Sample.dtsx"));
            Assert.That(etl.Password, Is.EqualTo("p@ssw0rd"));
        }

        [Test]
        public void Deserialize_EtlFromSqlServerInSetup_EtlXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<IEtlRunCommand>());
            var etl = ts.Tests[testNr].Setup.Commands[0] as EtlRunXml;

            Assert.That(etl, Is.Not.Null);
            Assert.That(etl.Server, Is.EqualTo("."));
            Assert.That(etl.Path, Is.EqualTo(@"Etl\"));
            Assert.That(etl.Name, Is.EqualTo("Sample"));
        }

        [Test]
        public void Deserialize_EtlFromFileInSystemUnderTest_EtlXml()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
            var etl = ts.Tests[testNr].Systems[0].BaseItem as EtlXml;

            Assert.That(etl, Is.Not.Null);
            Assert.That(etl.Path, Is.EqualTo(@"/Etl/"));
            Assert.That(etl.Name, Is.EqualTo("Sample.dtsx"));
        }

        [Test]
        public void Deserialize_EtlFromSqlServerInSystemUnderTest_EtlXml()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
            var etl = ts.Tests[testNr].Systems[0].BaseItem as EtlXml;

            Assert.That(etl, Is.Not.Null);
            Assert.That(etl.Server, Is.EqualTo("."));
            Assert.That(etl.Path, Is.EqualTo(@"Etl\"));
            Assert.That(etl.Name, Is.EqualTo("Sample"));
        }

        [Test]
        public void Deserialize_WithParameters_EtlXml()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0].BaseItem, Is.InstanceOf<EtlXml>());
            var etl = ts.Tests[testNr].Systems[0].BaseItem as EtlXml;
            var parameters = etl.Parameters;

            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters, Has.Count.EqualTo(2));
            Assert.That(parameters, Has.Member(new EtlParameter("param1", "value1")));
            Assert.That(parameters, Has.Member(new EtlParameter("param2", "value2")));
        }

        [Test]
        public void Deserialize_SetupWithParameters_EtlXml()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.InstanceOf<EtlRunXml>());
            var etl = ts.Tests[testNr].Setup.Commands[0] as EtlRunXml;
            var parameters = etl.Parameters;

            Assert.That(parameters, Is.Not.Null);
            Assert.That(parameters, Has.Count.EqualTo(2));
            Assert.That(parameters, Has.Member(new EtlParameter("param1", "value1")));
            Assert.That(parameters, Has.Member(new EtlParameter("param2", "value2")));
        }
    }
}
