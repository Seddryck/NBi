#region Using directives
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Constraints;
using NUnit.Framework;
using NBi.Xml.Constraints.Comparer;
using NBi.Core.ResultSet;
using NBi.Xml.Settings;
using NBi.Core.Evaluate;
using NBi.Xml.Items.Calculation;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
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
            var comparison = allRows.Predication;

            Assert.That(comparison.ColumnIndex, Is.EqualTo(-1));
            Assert.That(comparison.Operand, Is.EqualTo("ModDepId"));
            Assert.That(comparison.Not, Is.EqualTo(false));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Predicate, Is.TypeOf<MoreThanXml>());
            var moreThan = comparison.Predicate as MoreThanXml;
            Assert.That(moreThan.Value, Is.EqualTo("10"));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyVariables()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var aliases = allRows.Aliases;

            Assert.That(aliases, Has.Count.EqualTo(1));
            Assert.That(aliases[0].Name, Is.EqualTo("DeptId"));
            Assert.That(aliases[0].Column, Is.EqualTo(0));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyNullComparer()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<EmptyXml>());
            var emptyPredicate = predicate.Predicate as EmptyXml;
            Assert.That(emptyPredicate.OrNull, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyAliases()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var aliases = allRows.Aliases;

            Assert.That(aliases, Has.Count.EqualTo(2));
            Assert.That(aliases[0].Name, Is.EqualTo("Name"));
            Assert.That(aliases[0].Column, Is.EqualTo(0));
            Assert.That(aliases[1].Name, Is.EqualTo("Name2"));
            Assert.That(aliases[1].Column, Is.EqualTo(1));
        }


        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyStartsWithComparer()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<StartsWithXml>());
            var cpr = predicate.Predicate as StartsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyEndsWithComparer()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<EndsWithXml>());
            var cpr = predicate.Predicate as EndsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyContainsComparer()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<ContainsXml>());
            var cpr = predicate.Predicate as ContainsXml;
            Assert.That(cpr.IgnoreCase, Is.True);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMatchesRegexComparer()
        {
            int testNr = 5;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<MatchesRegexXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyLowerCaseComparer()
        {
            int testNr = 6;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<LowerCaseXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyUpperCaseComparer()
        {
            int testNr = 7;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Name"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<UpperCaseXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyWithinRangeComparer()
        {
            int testNr = 8;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.ColumnIndex, Is.EqualTo(-1));
            Assert.That(predicate.Operand, Is.EqualTo("Value"));
            Assert.That(predicate.Not, Is.EqualTo(false));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(predicate.Predicate, Is.TypeOf<WithinRangeXml>());
            var cpr = predicate.Predicate as WithinRangeXml;
            Assert.That(cpr.Value, Is.EqualTo("[10;30]"));
        }

        [Test]
        public void Serialize_AllRowsXml_OnlyAliasNoVariable()
        {
            var allRowsXml = new AllRowsXml();
            allRowsXml.InternalAliasesOld = new List<AliasXml>()
            {
                new AliasXml() {Column = 1, Name="Col1"},
                new AliasXml() {Column = 0, Name="Col2"}
            };
            allRowsXml.Predication = new PredicationXml();
            
            var serializer = new XmlSerializer(typeof(AllRowsXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, allRowsXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("alias"));
            Assert.That(content, Is.Not.StringContaining("variable"));
        }

        

    }
}
