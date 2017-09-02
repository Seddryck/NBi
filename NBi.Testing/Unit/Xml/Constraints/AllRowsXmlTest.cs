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
    public class AllRowsXmlTest
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.AllRowsXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyAllRows()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var comparison = allRows.Predicate;

            Assert.That(comparison.ColumnIndex, Is.EqualTo(-1));
            Assert.That(comparison.Name, Is.EqualTo("ModDepId"));
            Assert.That(comparison.Not, Is.EqualTo(false));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Comparer, Is.TypeOf<MoreThanXml>());
            var moreThan = comparison.Comparer as MoreThanXml;
            Assert.That(moreThan.Value, Is.EqualTo("10"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyNullComparer()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<EmptyXml>());
            var emptyPredicate = predicate.Comparer as EmptyXml;
            Assert.That(emptyPredicate.OrNull, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyStartsWithComparer()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<StartsWithXml>());
            var cpr = predicate.Comparer as StartsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyEndsWithComparer()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<EndsWithXml>());
            var cpr = predicate.Comparer as EndsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyContainsComparer()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<ContainsXml>());
            var cpr = predicate.Comparer as ContainsXml;
            Assert.That(cpr.IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMatchesRegexComparer()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<MatchesRegexXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyLowerCaseComparer()
        {
            int testNr = 6;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<LowerCaseXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyUpperCaseComparer()
        {
            int testNr = 7;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predicate;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Name, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Comparer, Is.TypeOf<UpperCaseXml>());
        }

    }
}
