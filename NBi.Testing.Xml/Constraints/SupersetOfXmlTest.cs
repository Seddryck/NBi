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
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class SupersetOfXmlTest : BaseXmlTest
{

    [Test]
    public void DeserializeSupersetResultSet_QueryFile0_Inline()
    {
        int testNr = 0;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ResultSetOld, Is.Not.Null);
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ResultSetOld!.Rows, Has.Count.EqualTo(2));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ResultSetOld!.Rows[0].Cells, Has.Count.EqualTo(3));
    }

    [Test]
    public void DeserializeSupersetResultSet_QueryFile1_ExternalFile()
    {
        int testNr = 1;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ResultSetOld, Is.Not.Null);
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ResultSetOld!.File, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void DeserializeSupersetKey_QueryFile2_List()
    {
        int testNr = 2;
        
        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).KeysDef, Is.EqualTo(SettingsOrdinalResultSet.KeysChoice.First));
    }

    [Test]
    public void DeserializeSupersetKey_QueryFile3_List()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef, Has.Count.EqualTo(2));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Index").EqualTo(3));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0], Has.Property("Tolerance").EqualTo("10"));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1], Has.Property("Index").EqualTo(4));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1], Has.Property("Type").EqualTo(ColumnType.Boolean));
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile4_List()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).Query, Is.TypeOf<QueryXml>());

        var connStr = ((SupersetOfXml)ts.Tests[testNr].Constraints[0]).Query!.ConnectionString;
        Assert.That(connStr, Is.Not.Empty);
        Assert.That(connStr, Contains.Substring("Reference"));

        var query = ((SupersetOfXml)ts.Tests[testNr].Constraints[0]).Query!.InlineQuery;
        Assert.That(query, Is.Not.Empty);
        Assert.That(query, Contains.Substring("select top 2 [Name]"));
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile5_List()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ValuesDef, Is.EqualTo(SettingsOrdinalResultSet.ValuesChoice.Last));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).Tolerance, Is.EqualTo("100"));

        
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile7_RoundingAttributeRead()
    {
        int testNr = 7;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].RoundingStyle, Is.EqualTo(Rounding.RoundingStyle.Round));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].RoundingStep, Is.EqualTo("100"));

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].RoundingStyle, Is.EqualTo(Rounding.RoundingStyle.Floor));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].RoundingStep, Is.EqualTo("00:15:00"));
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile8_ToleranceAttributeRead()
    {
        int testNr = 8;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[1].Tolerance, Is.EqualTo("16%"));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[2].Tolerance, Is.EqualTo("1.12:00:00"));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[3].Tolerance, Is.EqualTo("00:15:00"));
        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[4].Tolerance, Is.EqualTo("00:00:00.125"));
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile8_ValuesDefaulTypeWithoutSpecificValueRead()
    {
        int testNr = 8;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.Numeric));
    }

    [Test]
    public void DeserializeSupersetQuery_QueryFile8_ValuesDefaulTypeRead()
    {
        int testNr = 9;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());

        Assert.That(((SupersetOfXml)ts.Tests[testNr].Constraints[0]).ValuesDefaultType, Is.EqualTo(ColumnType.DateTime));
    }

    [Test]
    public void DeserializeSupersetQuery_DefaultValue_Transformation()
    {
        int testNr = 10;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        var ctr = (SupersetOfXml)ts.Tests[testNr].Constraints[0];


        Assert.That(ctr.ColumnsDef[0].Transformation, Is.TypeOf<LightTransformXml>());
        var transfo = (LightTransformXml)ctr.ColumnsDef[0].Transformation!;

        Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
        Assert.That(transfo.OriginalType, Is.EqualTo(ColumnType.Text));
        Assert.That(transfo.Code, Is.EqualTo("value.Substring(2)"));
    }

    [Test]
    public void DeserializeSupersetQuery_NoDefaultValue_Transformation()
    {
        int testNr = 10;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        var ctr = (SupersetOfXml)ts.Tests[testNr].Constraints[0];


        Assert.That(ctr.ColumnsDef[1].Transformation, Is.TypeOf<LightTransformXml>());
        var transfo = (LightTransformXml)ctr.ColumnsDef[1].Transformation!;

        Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
        Assert.That(transfo.OriginalType, Is.EqualTo(ColumnType.DateTime));
        Assert.That(transfo.Code, Is.EqualTo("String.Format(\"{0:00}.{1}\", value.Month, value.Year)"));
    }

    [Test]
    public void DeserializeSupersetQuery_BehaviorSingleRow_SingleRow()
    {
        int testNr = 11;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<SupersetOfXml>());
        var ctr = (SupersetOfXml)ts.Tests[testNr].Constraints[0];

        Assert.That(ctr.Behavior, Is.EqualTo(SupersetOfXml.ComparisonBehavior.SingleRow));
    }
}
