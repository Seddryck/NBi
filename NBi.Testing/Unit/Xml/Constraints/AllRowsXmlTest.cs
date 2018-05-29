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
            Assert.That((comparison.Operand as ColumnNameIdentifier).Name, Is.EqualTo("ModDepId"));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Predicate, Is.TypeOf<MoreThanXml>());
            var moreThan = comparison.Predicate as MoreThanXml;
            Assert.That(moreThan.Value, Is.EqualTo("10"));
            Assert.That(moreThan.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));
            Assert.That(predicate.Predicate, Is.TypeOf<EmptyXml>());

            var emptyPredicate = predicate.Predicate as EmptyXml;
            Assert.That(emptyPredicate.OrNull, Is.True);
            Assert.That(emptyPredicate.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<StartsWithXml>());
            var cpr = predicate.Predicate as StartsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
            Assert.That(cpr.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));
            Assert.That(predicate.Predicate, Is.TypeOf<EndsWithXml>());

            var cpr = predicate.Predicate as EndsWithXml;
            Assert.That(cpr.IgnoreCase, Is.False);
            Assert.That(cpr.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<ContainsXml>());
            var cpr = predicate.Predicate as ContainsXml;
            Assert.That(cpr.IgnoreCase, Is.True);
            Assert.That(cpr.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<MatchesRegexXml>());
            Assert.That(predicate.Predicate.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<LowerCaseXml>());
            Assert.That(predicate.Predicate.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Name"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

            Assert.That(predicate.Predicate, Is.TypeOf<UpperCaseXml>());
            Assert.That(predicate.Predicate.Not, Is.False);
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
            Assert.That((predicate.Operand as ColumnNameIdentifier).Name, Is.EqualTo("Value"));
            Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(predicate.Predicate, Is.TypeOf<WithinRangeXml>());
            var cpr = predicate.Predicate as WithinRangeXml;
            Assert.That(cpr.Value, Is.EqualTo("[10;30]"));
            Assert.That(cpr.Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyWithinListComparer()
        {
            int testNr = 9;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.Predicate, Is.AssignableTo<AnyOfXml>());
            var cpr = predicate.Predicate as AnyOfXml;
            Assert.That(cpr.Values, Has.Count.EqualTo(3));
        }


        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyAnyOfComparer()
        {
            int testNr = 10;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var predicate = allRows.Predication;

            Assert.That(predicate.Predicate, Is.AssignableTo<AnyOfXml>());
            var cpr = predicate.Predicate as AnyOfXml;
            Assert.That(cpr.Values, Has.Count.EqualTo(3));
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyMultipleExpressions()
        {
            int testNr = 11;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var allRows = ts.Tests[testNr].Constraints[0] as AllRowsXml;
            var expressions = allRows.Expressions;

            Assert.That(allRows.Expressions, Is.AssignableTo<IEnumerable<ExpressionXml>>());
            Assert.That(allRows.Expressions, Has.Count.EqualTo(2));
        }

        [Test]
        public void Serialize_AllRowsXml_OnlyAliasNoVariable()
        {
            var allRowsXml = new AllRowsXml
#pragma warning disable 0618
            {
                InternalAliasesOld = new List<AliasXml>()
            {
                new AliasXml() {Column = 1, Name="Col1"},
                new AliasXml() {Column = 0, Name="Col2"}
            },
                Predication = new PredicationXml()
            };
#pragma warning restore 0618

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

        [Test]
        public void Serialize_AllRowsXml_AnyOfXml()
        {
            var allRowsXml = new AllRowsXml
            {
                Predication = new PredicationXml()
                {
                    Predicate = new AnyOfXml()
                    {
                        Values = new List<string>() { "first", "second" }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(AllRowsXml));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            serializer.Serialize(writer, allRowsXml);
            var content = Encoding.UTF8.GetString(stream.ToArray());
            writer.Close();
            stream.Close();

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("any-of"));
            Assert.That(content, Is.StringContaining("item"));
            Assert.That(content, Is.StringContaining("first"));
            Assert.That(content, Is.StringContaining("second"));
        }

        [Test]
        public void Serialize_ExecutionXml_NoColumnIndex()
        {
            var allRowsXml = new AllRowsXml
            {
                Expressions = new List<ExpressionXml>()
                {
                    new ExpressionXml()
                    {
                        Value = "a + b = c",
                        Type = ColumnType.Boolean,
                        Name = "calculate"
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(AllRowsXml));
            var content = string.Empty;
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    serializer.Serialize(writer, allRowsXml);
                content = Encoding.UTF8.GetString(stream.ToArray());
            }

            Debug.WriteLine(content);

            Assert.That(content, Is.StringContaining("expression"));
            Assert.That(content, Is.StringContaining("type"));
            Assert.That(content, Is.StringContaining("name"));
            Assert.That(content, Is.StringContaining(">a + b = c<"));
            Assert.That(content, Is.Not.StringContaining("column-type"));
            Assert.That(content, Is.Not.StringContaining("column-index"));
            Assert.That(content, Is.Not.StringContaining("tolerance"));
        }



    }
}
