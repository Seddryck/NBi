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
    public class QueryXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.QueryXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_QueryWithoutParams_QueryXml()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Systems[0], Is.TypeOf<ExecutionXml>());
            Assert.That(((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem, Is.TypeOf<QueryXml>());
            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query, Is.Not.Null);
            Assert.That(query.Parameters, Has.Count.EqualTo(0));
        }

        [Test]
        public void Deserialize_QueryWithTwoParams_QueryXml()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query.GetQuery(), Is.StringContaining("select myColumn from myTable where myFirstField=@FirstField and 1=@NonEmpty"));

            Assert.That(query.Parameters, Is.Not.Null);
            Assert.That(query.Parameters, Is.Not.Empty);
            Assert.That(query.Parameters, Has.Count.EqualTo(2));

            Assert.That(query.Parameters[0].Name, Is.EqualTo("FirstField"));
            Assert.That(query.Parameters[0].GetValue<string>(), Is.EqualTo("Identifier"));

            Assert.That(query.Parameters[1].Name, Is.EqualTo("NonEmpty"));
            Assert.That(query.Parameters[1].GetValue<int>(), Is.EqualTo(1));
        }

        [Test]
        public void Deserialize_QueryWithOneParamAndSqlType_QueryXml()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            var query = (QueryXml)((ExecutionXml)ts.Tests[testNr].Systems[0]).BaseItem;

            Assert.That(query.Parameters, Is.Not.Null);
            Assert.That(query.Parameters, Is.Not.Empty);
            Assert.That(query.Parameters, Has.Count.EqualTo(1));

            Assert.That(query.Parameters[0].SqlType.ToLower(), Is.EqualTo("varchar(255)"));
        }
    }
}
