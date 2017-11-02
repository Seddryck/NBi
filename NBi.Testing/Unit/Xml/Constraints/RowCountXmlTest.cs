#region Using directives
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.ResultSet;
using NBi.Xml.Items.Calculation;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
#endregion

namespace NBi.Testing.Unit.Xml.Constraints
{
    [TestFixture]
    public class RowCountXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.RowCountXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyRowCount()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<RowCountXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

         [Test]
        public void Deserialize_SampleFile_ReadCorrectlyEqual()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.Equal, Is.Not.Null);
            Assert.That(rowCount.Equal, Is.TypeOf<EqualXml>());
            Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.Equal));
            
            var comparer = rowCount.Equal as PredicateXml;
            Assert.That(comparer.Value, Is.EqualTo("2"));
        }

         [Test]
        public void Deserialize_SampleFile_ReadCorrectlyLessThan()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.LessThan, Is.Not.Null);
            Assert.That(rowCount.LessThan, Is.TypeOf<LessThanXml>());
            Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.LessThan));

            var comparer = rowCount.Comparer as MoreLessThanPredicateXml;
            Assert.That(comparer.Value, Is.EqualTo("3"));
            Assert.That(comparer.OrEqual, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMoreThan()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.Not, Is.True);
            Assert.That(rowCount.MoreThan, Is.Not.Null);
            Assert.That(rowCount.MoreThan, Is.TypeOf<MoreThanXml>());
            Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.MoreThan));

            var comparer = rowCount.Comparer as MoreLessThanPredicateXml;
            Assert.That(comparer.Value, Is.EqualTo("3"));
            Assert.That(comparer.OrEqual, Is.True);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Deserialize_SampleFile_ReadCorrectlyPredicateWhenNull(int testNr)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.Filter, Is.Null);
        }

        [Test()]
        [TestCase(3)]
        [TestCase(4)]
        public void Deserialize_SampleFile_ReadCorrectlyPredicate(int testNr)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.Filter, Is.Not.Null);
            Assert.That(rowCount.Filter.Aliases, Is.Not.Null);
            Assert.That(rowCount.Filter.Aliases, Has.Count.EqualTo(1));
            Assert.That(rowCount.Filter.Predication, Is.Not.Null);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlySimpleComparer()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var comparison = rowCount.Filter.Predication;

            //Assert.That(comparison.ColumnIndex, Is.EqualTo(2));
            Assert.That(comparison.Not, Is.EqualTo(true));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(comparison.Predicate, Is.TypeOf<EqualXml>());
            var equal = comparison.Predicate as EqualXml;
            Assert.That(equal.Value, Is.EqualTo("N/A"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var comparison = rowCount.Filter.Predication;

            Assert.That(comparison.ColumnIndex, Is.EqualTo(-1));
            Assert.That(comparison.Operand, Is.EqualTo("ModDepId"));
            Assert.That(comparison.Not, Is.EqualTo(false));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Predicate, Is.TypeOf<LessThanXml>());
            var lessThan = comparison.Predicate as LessThanXml;
            Assert.That(lessThan.Value, Is.EqualTo("1"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyVariables()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var variables = rowCount.Filter.Aliases;

            Assert.That(variables, Has.Count.EqualTo(1));
            Assert.That(variables.ElementAt(0).Name, Is.EqualTo("DeptId"));
            Assert.That(variables.ElementAt(0).Column, Is.EqualTo(0));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormula()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var formula = rowCount.Filter.Expression;

            Assert.That(formula.Name, Is.EqualTo("LogDepId"));
            Assert.That(formula.Value, Is.StringContaining("Log10(DepId)"));
        }

        [Test]
        public void Serialize_WithLessThanAndFilter_LessThanBeforeFilter()
        {
            var rowCountXml = new RowCountXml()
            {
                Filter = new FilterXml(),
                LessThan = new LessThanXml()
            };

            var serializer = new XmlSerializer(typeof(RowCountXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, rowCountXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<filter"));
            Assert.That(content, Is.StringContaining("<less-than"));

            Assert.That(content, Is.StringMatching(@".*<filter.*/>[\r\n]*.*<less-than.*/>.*"));
        }

        [Test]
        public void Serialize_WithLessThanAndFilter_OnlyAliasNoVariable()
        {
            var rowCountXml = new RowCountXml()
            {
                Filter = new FilterXml()
                {
                    InternalAliases = new List<AliasXml>()
                    {
                        new AliasXml() {Column = 1, Name="Col1"},
                        new AliasXml() {Column = 0, Name="Col2"}
                    }
                },
                LessThan = new LessThanXml()
            };

            var serializer = new XmlSerializer(typeof(RowCountXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, rowCountXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("<alias"));
            Assert.That(content, Is.Not.StringContaining("<variable"));
        }
    }
}
