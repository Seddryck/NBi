#region Using directives
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.ResultSet;
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
            
            var comparer = rowCount.Equal as AbstractComparerXml;
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

            var comparer = rowCount.Comparer as AbstractMoreLessThanXml;
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

            var comparer = rowCount.Comparer as AbstractMoreLessThanXml;
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
            Assert.That(rowCount.Predicate, Is.Null);
        }

        [Test()]
        [TestCase(3)]
        [TestCase(4)]
        public void Deserialize_SampleFile_ReadCorrectlyPredicate(int testNr)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            Assert.That(rowCount.Predicate, Is.Not.Null);
            Assert.That(rowCount.Predicate.Comparison, Is.Not.Null);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlySimpleComparer()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var comparison = rowCount.Predicate.Comparison;

            Assert.That(comparison.ColumnIndex, Is.EqualTo(2));
            Assert.That(comparison.Not, Is.EqualTo(true));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(comparison.Comparer, Is.TypeOf<EqualXml>());
            var equal = comparison.Comparer as EqualXml;
            Assert.That(equal.Value, Is.EqualTo("N/A"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var comparison = rowCount.Predicate.Comparison;

            Assert.That(comparison.ColumnIndex, Is.EqualTo(-1));
            Assert.That(comparison.Formula, Is.EqualTo("ModDepId"));
            Assert.That(comparison.Not, Is.EqualTo(false));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Comparer, Is.TypeOf<LessThanXml>());
            var lessThan = comparison.Comparer as LessThanXml;
            Assert.That(lessThan.Value, Is.EqualTo("1"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyVariables()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var variables = rowCount.Predicate.Variables;

            Assert.That(variables, Has.Count.EqualTo(1));
            Assert.That(variables[0].Name, Is.EqualTo("DeptId"));
            Assert.That(variables[0].Column, Is.EqualTo(0));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormula()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
            var formula = rowCount.Predicate.Formula;

            Assert.That(formula.Name, Is.EqualTo("LogDepId"));
            Assert.That(formula.Value, Is.StringContaining("Log10(DepId)"));
        }
    }
}
