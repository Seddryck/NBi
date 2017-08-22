#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Xml.Items.ResultSet;
using NBi.Core.Transformation;
#endregion

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class EqualToXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.EqualToXmlTestSuite.xml"))
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
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).KeysDef, Is.EqualTo(SettingsResultSetComparisonByIndex.KeysChoice.First));
        }

        [Test]
        public void DeserializeEqualToKey_QueryFile3_List()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef, Has.Count.EqualTo(2));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Index").EqualTo(3));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Tolerance").EqualTo("10"));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1], Has.Property("Index").EqualTo(4));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1], Has.Property("Type").EqualTo(ColumnType.Boolean));
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
            Assert.That(query, Contains.Substring("select top 2 [Name]"));

            var cmd = ((EqualToXml)ts.Tests[testNr].Constraints[0]).GetCommand();
            Assert.That(cmd, Is.Not.Null);
            Assert.That(cmd.Connection.ConnectionString, Contains.Substring("Adventure"));
            Assert.That(cmd.CommandText, Contains.Substring("select top 2 [Name]"));
            
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile5_List()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDef, Is.EqualTo(SettingsResultSetComparisonByIndex.ValuesChoice.Last));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Tolerance, Is.EqualTo("100"));

            
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

        [Test]
        public void DeserializeEqualToQuery_QueryFile7_RoundingAttributeRead()
        {
            int testNr = 7;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].RoundingStyle, Is.EqualTo(Rounding.RoundingStyle.Round));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].RoundingStep, Is.EqualTo("100"));

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].RoundingStyle, Is.EqualTo(Rounding.RoundingStyle.Floor));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].RoundingStep, Is.EqualTo("00:15:00"));
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile8_ToleranceAttributeRead()
        {
            int testNr = 8;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].Tolerance, Is.EqualTo("16%"));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].Tolerance, Is.EqualTo("1.12:00:00"));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[3].Tolerance, Is.EqualTo("00:15:00"));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[4].Tolerance, Is.EqualTo("00:00:00.125"));
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile8_ValuesDefaulTypeWithoutSpecificValueRead()
        {
            int testNr = 8;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.Numeric));
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile8_ValuesDefaulTypeRead()
        {
            int testNr = 9;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.DateTime));
        }

        [Test]
        public void DeserializeEqualToQuery_DefaultValue_Transformation()
        {
            int testNr = 10;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;


            Assert.That(ctr.ColumnsDef[0].Transformation, Is.TypeOf<TransformationXml>());
            var transfo = ctr.ColumnsDef[0].Transformation as TransformationXml;

            Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
            Assert.That(transfo.OriginalType, Is.EqualTo(ColumnType.Text));
            Assert.That(transfo.Code, Is.EqualTo("value.Substring(2)"));
        }

        [Test]
        public void DeserializeEqualToQuery_NoDefaultValue_Transformation()
        {
            int testNr = 10;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;


            Assert.That(ctr.ColumnsDef[1].Transformation, Is.TypeOf<TransformationXml>());
            var transfo = ctr.ColumnsDef[1].Transformation as TransformationXml;

            Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
            Assert.That(transfo.OriginalType, Is.EqualTo(ColumnType.DateTime));
            Assert.That(transfo.Code, Is.EqualTo("String.Format(\"{0:00}.{1}\", value.Month, value.Year)"));
        }

        [Test]
        public void DeserializeEqualToQuery_BehaviorSingleRow_SingleRow()
        {
            int testNr = 11;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;

            Assert.That(ctr.Behavior, Is.EqualTo(EqualToXml.ComparisonBehavior.SingleRow));
        }
    }
}
