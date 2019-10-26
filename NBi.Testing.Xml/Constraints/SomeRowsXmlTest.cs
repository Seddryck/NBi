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

namespace NBi.Testing.Xml.Unit.Constraints
{
    [TestFixture]
    public class SomeRowsXmlTest : BaseXmlTest
    {

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyNoRows()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints, Has.Count.EqualTo(1));
            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SomeRowsXml>());
            Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
        }

        [Test]
        public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            var someRows = ts.Tests[testNr].Constraints[0] as SomeRowsXml;
            var comparison = someRows.Predication;

            Assert.That((comparison.Operand as ColumnNameIdentifier).Name, Is.EqualTo("ModDepId"));
            Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

            Assert.That(comparison.Predicate, Is.TypeOf<MoreThanXml>());
            Assert.That(comparison.Predicate.Not, Is.EqualTo(false));
            var moreThan = comparison.Predicate as MoreThanXml;
            Assert.That(moreThan.Reference, Is.EqualTo("10"));
        }

    }
}
