#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Xml.Items.ResultSet;
using NBi.Core.Transformation;
using NBi.Xml.Items.Alteration.Transform;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
#endregion

namespace NBi.Testing.Xml.Unit.Constraints
{
    [TestFixture]
    public class EqualToXmlTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
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
                                           .GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Resources.EqualToXmlTestSuite.xml"))
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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSetOld, Is.Not.Null);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSetOld.Rows, Has.Count.EqualTo(2));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSetOld.Rows[0].Cells, Has.Count.EqualTo(3));
        }

        [Test]
        public void DeserializeEqualToResultSet_QueryFile1_ExternalFile()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSetOld, Is.Not.Null);
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ResultSetOld.File, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void DeserializeEqualToKey_QueryFile2_List()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).KeysDef, Is.EqualTo(SettingsOrdinalResultSet.KeysChoice.First));
        }

        [Test]
        public void DeserializeEqualToKey_QueryFile3_List()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDef, Is.EqualTo(SettingsOrdinalResultSet.ValuesChoice.Last));
            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).Tolerance, Is.EqualTo("100"));


        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile7_RoundingAttributeRead()
        {
            int testNr = 7;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());

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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());

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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.Numeric));
        }

        [Test]
        public void DeserializeEqualToQuery_QueryFile8_ValuesDefaulTypeRead()
        {
            int testNr = 9;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());

            Assert.That(((EqualToXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.DateTime));
        }

        [Test]
        public void DeserializeEqualToQuery_DefaultValue_Transformation()
        {
            int testNr = 10;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;


            Assert.That(ctr.ColumnsDef[0].Transformation, Is.TypeOf<LightTransformXml>());
            var transfo = ctr.ColumnsDef[0].Transformation as LightTransformXml;

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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;


            Assert.That(ctr.ColumnsDef[1].Transformation, Is.TypeOf<LightTransformXml>());
            var transfo = ctr.ColumnsDef[1].Transformation as LightTransformXml;

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

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;

            Assert.That(ctr.Behavior, Is.EqualTo(EqualToXml.ComparisonBehavior.SingleRow));
        }

        [Test]
        public void DeserializeEqualToQuery_FullFeatured_Correct()
        {
            int testNr = 12;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.AssignableTo<EqualToXml>());
            var ctr = ts.Tests[testNr].Constraints[0] as EqualToXml;
            Assert.That(ctr.ResultSet, Is.TypeOf<ResultSetSystemXml>());
            var rs = ctr.ResultSet as ResultSetSystemXml;
            Assert.That(rs.File.Path, Is.EqualTo(@"C:\bar.txt"));
            Assert.That(rs.File.IfMissing.File.Path, Is.EqualTo(@"C:\foo.txt"));
            Assert.That(rs.Alteration.Filters, Has.Count.EqualTo(1));
            Assert.That(rs.Alteration.Filters[0].Predication.Predicate.ComparerType, Is.EqualTo(ComparerType.MoreThan));
        }

        [Test]
        public void SerializeEqualToQuery_Transform_SingleRow()
        {

            // Create an instance of the XmlSerializer specifying type and namespace.
            var cdXml = new ColumnDefinitionXml()
            {
                Index = 1,
                Role = ColumnRole.Key,
                TransformationInner = new LightTransformXml()
                {
                    Language = LanguageType.CSharp,
                    OriginalType = ColumnType.Numeric,
                    Code = "value * 1000"
                }
            };

            var serializer = new XmlSerializer(typeof(ColumnDefinitionXml));
            var content = string.Empty;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    serializer.Serialize(writer, cdXml);
                content = Encoding.UTF8.GetString(stream.ToArray());
            }

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<transform "));
            Assert.That(content, Does.Not.Contain("index=\"0\""));
            Assert.That(content, Does.Contain("value * 1000"));
            Assert.That(content, Does.Not.Contain("Intern"));
        }

        [Test]
        public void SerializeEqualToQuery_EqualTo_NewSyntax()
        {

            // Create an instance of the XmlSerializer specifying type and namespace.
            var cdXml = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>()
                {
                    new EqualToXml()
                    {
                        Query = new QueryXml()
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(TestXml));
            var content = string.Empty;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    serializer.Serialize(writer, cdXml);
                content = Encoding.UTF8.GetString(stream.ToArray());
            }

            Debug.WriteLine(content);

            Assert.That(content, Does.Contain("<equal-to"));
        }
    }
}
