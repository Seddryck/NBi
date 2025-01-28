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

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class NoRowsXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyNoRows()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<NoRowsXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var noRow = (NoRowsXml)ts.Tests[testNr].Constraints[0]!;
        var comparison = noRow.Predication;

        Assert.That(((ColumnNameIdentifier)comparison.Operand!).Name, Is.EqualTo("ModDepId"));
        Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

        Assert.That(comparison.Predicate, Is.TypeOf<MoreThanXml>());
        Assert.That(comparison.Predicate!.Not, Is.EqualTo(false));
        var moreThan = (MoreThanXml)comparison.Predicate!;
        Assert.That(moreThan.Reference, Is.EqualTo("10"));
    }

}
