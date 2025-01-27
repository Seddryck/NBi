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

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class RowCountXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyRowCount()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<RowCountXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }


    //TODO ROW-COUNT
    // [Test]
    //public void Deserialize_SampleFile_ReadCorrectlyEqual()
    //{
    //    int testNr = 0;

    //    // Create an instance of the XmlSerializer specifying type and namespace.
    //    var ts = DeserializeSample();
    //    var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
    //    Assert.That(rowCount.Equal, Is.Not.Null);
    //    Assert.That(rowCount.Equal, Is.TypeOf<EqualXml>());
    //    Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.Equal));
        
    //    var comparer = rowCount.Equal as PredicateXml;
    //    Assert.That(comparer.Value, Is.EqualTo("2"));
    //}

    // [Test]
    //public void Deserialize_SampleFile_ReadCorrectlyLessThan()
    //{
    //    int testNr = 1;

    //    // Create an instance of the XmlSerializer specifying type and namespace.
    //    var ts = DeserializeSample();
    //    var rowCount = ts.Tests[testNr].Constraints[0] as RowCountXml;
    //    Assert.That(rowCount.LessThan, Is.Not.Null);
    //    Assert.That(rowCount.LessThan, Is.TypeOf<LessThanXml>());
    //    Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.LessThan));

    //    var comparer = rowCount.Comparer as MoreLessThanPredicateXml;
    //    Assert.That(comparer.Value, Is.EqualTo("3"));
    //    Assert.That(comparer.OrEqual, Is.False);
    //}

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyMoreThan()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        Assert.That(rowCount.Not, Is.True);
        Assert.That(rowCount.MoreThan, Is.Not.Null);
        Assert.That(rowCount.MoreThan, Is.TypeOf<MoreThanXml>());
        Assert.That(rowCount.Comparer, Is.EqualTo(rowCount.MoreThan));

        var comparer = (MoreLessThanPredicateXml)rowCount.Comparer!;
        Assert.That(comparer.Reference, Is.EqualTo("3"));
        Assert.That(comparer.OrEqual, Is.True);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void Deserialize_SampleFile_ReadCorrectlyPredicateWhenNull(int testNr)
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        Assert.That(rowCount.Filter, Is.Null);
    }

    [Test()]
    [TestCase(3)]
    [TestCase(4)]
    public void Deserialize_SampleFile_ReadCorrectlyPredicate(int testNr)
    {
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        Assert.That(rowCount.Filter, Is.Not.Null);
        Assert.That(rowCount.Filter!.Aliases, Is.Not.Null);
        Assert.That(rowCount.Filter.Aliases, Has.Count.EqualTo(1));
        Assert.That(rowCount.Filter.Predication, Is.Not.Null);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlySimpleComparer()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        var comparison = rowCount.Filter!.Predication;

        Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Text));
        Assert.That(comparison.Predicate, Is.TypeOf<EqualXml>());
        var equal = (EqualXml)comparison.Predicate!;
        Assert.That(equal.Reference, Is.EqualTo("N/A"));
        Assert.That(equal.Not, Is.True);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        var comparison = rowCount.Filter!.Predication;

        Assert.That(((ColumnNameIdentifier)comparison.Operand!).Name, Is.EqualTo("ModDepId"));
        Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

        Assert.That(comparison.Predicate, Is.TypeOf<LessThanXml>());
        var lessThan = (LessThanXml)comparison.Predicate!;
        Assert.That(lessThan.Reference, Is.EqualTo("1"));
        Assert.That(lessThan.Not, Is.EqualTo(false));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyVariables()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        var variables = rowCount.Filter!.Aliases;

        Assert.That(variables, Has.Count.EqualTo(1));
        Assert.That(variables.ElementAt(0).Name, Is.EqualTo("DeptId"));
        Assert.That(variables.ElementAt(0).Column, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyFormula()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var rowCount = (RowCountXml)ts.Tests[testNr].Constraints[0];
        var formula = rowCount.Filter!.Expression;

        Assert.That(formula.Name, Is.EqualTo("LogDepId"));
        Assert.That(formula.Value, Does.Contain("Log10(DepId)"));
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

        Assert.That(content, Does.Contain("<filter"));
        Assert.That(content, Does.Contain("<less-than"));

        Assert.That(content, Does.Match(@".*<filter.*/>[\r\n]*.*<less-than.*/>.*"));
    }

    [Test]
    public void Serialize_WithLessThanAndFilter_OnlyAliasNoVariable()
    {
        var rowCountXml = new RowCountXml()
        {
            Filter = new FilterXml()
            {
                InternalAliases =
                [
                    new AliasXml() {Column = 1, Name="Col1"},
                    new AliasXml() {Column = 0, Name="Col2"}
                ]
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

        Assert.That(content, Does.Contain("<alias"));
        Assert.That(content, Does.Not.Contain("<variable"));
    }
}
