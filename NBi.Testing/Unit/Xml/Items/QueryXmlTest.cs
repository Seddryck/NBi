#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
#endregion

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
        public void DeserializeEqualToResultSet_QueryFile0_Inline()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSet, Is.Not.Null);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSet.Rows, Has.Count.EqualTo(2));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSet.Rows[0].Cells, Has.Count.EqualTo(3));
        }

        [Test]
        public void DeserializeEqualToResultSet_QueryFile1_ExternalFile()
        {
            int testNr = 1;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSet, Is.Not.Null);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSet.File, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToKey_QueryFile2_List()
        {
            int testNr = 2;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).KeysDef, Is.EqualTo(ResultSetComparisonSettings.KeysChoice.First));
        }

        [Test]
        public void DeserializeEqualToKey_QueryFile3_List()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef, Has.Count.EqualTo(1));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Index").EqualTo(3));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Tolerance").EqualTo(10));
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile4_List()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Query, Is.TypeOf<QueryXml>());

            var connStr = ((EqualToXml)ts.Tests[testNr].Constraints[0]).Query.GetConnectionString();
            Assert.That(connStr, Is.Not.Empty);
            Assert.That(connStr, Contains.Substring("Reference"));

            var query = ((EqualToXml)ts.Tests[testNr].Constraints[0]).Query.GetQuery();
            Assert.That(query, Is.Not.Empty);
            Assert.That(query, Contains.Substring("Top2Product"));

            var cmd = ((EqualToXml)ts.Tests[testNr].Constraints[0]).GetCommand();
            Assert.That(cmd, Is.Not.Null);
            Assert.That(cmd.Connection.ConnectionString, Contains.Substring("Reference"));
            Assert.That(cmd.CommandText, Contains.Substring("Top2Product"));
            
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile5_List()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDef, Is.EqualTo(ResultSetComparisonSettings.ValuesChoice.Last));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Tolerance, Is.EqualTo(100));

            
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile6_PersistanceAttributeRead()
        {
            int testNr = 6;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Persistance, Is.EqualTo(PersistanceChoice.OnlyIfFailed));
        }

    }
}
