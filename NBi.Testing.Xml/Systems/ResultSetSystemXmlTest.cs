using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Core.Transformation;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Items.Alteration;
using NBi.Xml.Items.Alteration.Conversion;
using NBi.Xml.Items.Alteration.Duplication;
using NBi.Xml.Items.Alteration.Extension;
using NBi.Xml.Items.Alteration.Lookup;
using NBi.Xml.Items.Alteration.Merging;
using NBi.Xml.Items.Alteration.Projection;
using NBi.Xml.Items.Alteration.Renaming;
using NBi.Xml.Items.Alteration.Reshaping;
using NBi.Xml.Items.Alteration.Summarization;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.Calculation.Grouping;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Systems;
using NBi.Xml.Variables.Sequence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Xml.Testing.Unit.Systems;

[TestFixture]
public class ResultSetSystemXmlTest : BaseXmlTest
{

    [Test]
    public void Deserialize_SampleFile_CsvFile()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs!.File.Path, Is.EqualTo("myFile.csv"));
    }

    [Test]
    public void Deserialize_SampleFileWithParser_CsvFile()
    {
        int testNr = 10;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

        Assert.That(rs!.File.Path, Is.EqualTo("myFile.csv"));
        Assert.That(rs.File.Parser!.Name, Is.EqualTo("tabular"));
    }

    [Test]
    public void Deserialize_SampleFileWithParserInline_CsvFile()
    {
        int testNr = 11;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs!.File.Path, Is.EqualTo("myFile.csv"));
        Assert.That(rs.File.Parser!.Name, Is.EqualTo("tabular"));
    }

    [Test]
    public void Deserialize_SampleFile_EmbeddedResultSet()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

        Assert.That(rs!.Content, Is.Not.Null);
        Assert.That(rs.Content.Rows, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_SampleFile_QueryFile()
    {
        int testNr = 2;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs!.Query, Is.Not.Null);
        Assert.That(rs.Query!.File, Is.EqualTo("myfile.sql"));
    }

    [Test]
    public void Deserialize_SampleFile_EmbeddedQuery()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

        Assert.That(rs!.Query, Is.Not.Null);
        Assert.That(rs.Query!.File, Is.Null.Or.Empty);

        Assert.That(rs.Query.InlineQuery, Is.EqualTo("select * from myTable;"));
    }

    [Test]
    public void Deserialize_SampleFile_AssemblyQuery()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs!.Query, Is.Not.Null);
        Assert.That(rs.Query!.Assembly, Is.Not.Null);

        Assert.That(rs.Query.Assembly!.Path, Is.EqualTo("NBi.Testing.dll"));
    }

    [Test]
    public void Deserialize_SampleFile_ReportQuery()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = ts.Tests[testNr].Systems[0] as ResultSetSystemXml;

        Assert.That(rs!.Query, Is.Not.Null);
        Assert.That(rs.Query!.Report, Is.Not.Null);

        Assert.That(rs.Query.Report!.Name, Is.EqualTo("MyReport"));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationFilter()
    {
        int testNr = 6;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs!.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(((FilterXml)rs.Alterations[0]).Predication, Is.Not.Null);
    }

    [Test]
    public void Deserialize_SampleFile_AlterationConvert()
    {
        int testNr = 7;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<ConvertXml>());
        var convert = (ConvertXml)rs.Alterations[0];

        Assert.That(convert!.Column, Is.EqualTo("#0"));
        Assert.That(convert.Converter, Is.TypeOf<TextToDateConverterXml>());
        Assert.That(convert.Converter.Culture, Is.EqualTo("fr-fr"));
        Assert.That(convert.Converter.DefaultValue, Is.Null);
    }

    [Test]
    public void Deserialize_SampleFile_AlterationTransformation()
    {
        int testNr = 8;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<TransformXml>());
        var alteration = rs.Alterations[0] as TransformXml;

        Assert.That(alteration!.Language, Is.EqualTo(LanguageType.CSharp));
        Assert.That(alteration.OriginalType, Is.EqualTo(ColumnType.Text));
        Assert.That(alteration.Identifier.Label, Is.EqualTo("#1"));
        Assert.That(alteration.Identifier, Is.TypeOf<ColumnOrdinalIdentifier>());
        Assert.That(((ColumnOrdinalIdentifier)alteration.Identifier).Ordinal, Is.EqualTo(1));
        Assert.That(alteration.Code.Trim(), Is.EqualTo("value.EndsWith(\".\") ? value : value + \".\""));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationRename()
    {
        int testNr = 9;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));
        var renaming = (RenamingXml)rs.Alterations[0];
        Assert.That(renaming.Identifier.Label, Is.EqualTo("#3"));
        Assert.That(renaming.NewName, Is.EqualTo("myNewName"));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationExtend()
    {
        int testNr = 12;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));
        var extend = (ExtendXml)rs.Alterations[0];

        Assert.That(extend.Identifier.Label, Is.EqualTo("[myNewColumn]"));
        Assert.That(extend.Script, Is.Not.Null);
        Assert.That(extend.Script.Language, Is.EqualTo(LanguageType.NCalc));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationSummarization()
    {
        int testNr = 13;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<SummarizeXml>());
        var summerize = (SummarizeXml)rs.Alterations[0];

        Assert.That(summerize.Aggregation, Is.Not.Null);
        Assert.That(summerize.Aggregation, Is.TypeOf<SumXml>());
        Assert.That(summerize.Aggregation.ColumnType, Is.EqualTo(ColumnType.Numeric));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationUnstack()
    {
        int testNr = 14;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<UnstackXml>());
        var unstack = (UnstackXml)rs.Alterations[0];

        Assert.That(unstack.Header, Is.Not.Null);
        Assert.That(unstack.Header, Is.TypeOf<HeaderXml>());
        Assert.That(unstack.GroupBy, Is.TypeOf<GroupByXml>());
    }

    [Test]
    public void Deserialize_SampleFile_AlterationProject()
    {
        int testNr = 15;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<ProjectXml>());
        var project = (ProjectXml)rs.Alterations[0];

        Assert.That(project.Columns, Is.Not.Null);
        Assert.That(project.Columns.Count, Is.EqualTo(2));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationProjectAway()
    {
        int testNr = 16;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<ProjectAwayXml>());
        var projectAway = (ProjectAwayXml)rs.Alterations[0];

        Assert.That(projectAway.Columns, Is.Not.Null);
        Assert.That(projectAway.Columns.Count, Is.EqualTo(2));
    }


    [Test]
    public void Deserialize_SampleFile_AlterationLookupReplace()
    {
        int testNr = 17;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<LookupReplaceXml>());
        var lookup = (LookupReplaceXml)rs.Alterations[0];

        Assert.That(lookup.Missing, Is.Not.Null);
        Assert.That(lookup.Missing.Behavior, Is.EqualTo(Behavior.Failure));

        Assert.That(lookup.Join, Is.Not.Null);
        Assert.That(lookup.ResultSet, Is.Not.Null);

        Assert.That(lookup.Replacement, Is.Not.Null);
        Assert.That(lookup.Replacement.Identifier.Label, Is.EqualTo("#1"));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationUnion()
    {
        int testNr = 18;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<UnionXml>());
        var union = (UnionXml)rs.Alterations[0];

        Assert.That(union.ResultSet, Is.Not.Null);
        Assert.That(union.ColumnIdentity, Is.EqualTo(ColumnIdentity.Name));
    }

    [Test]
    public void Deserialize_SampleFile_AlterationDuplicate()
    {
        int testNr = 19;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Alterations, Is.Not.Null);
        Assert.That(rs.Alterations, Has.Count.EqualTo(1));

        Assert.That(rs.Alterations[0], Is.Not.Null);
        Assert.That(rs.Alterations[0], Is.TypeOf<DuplicateXml>());
        var duplicate = (DuplicateXml)rs.Alterations[0];

        Assert.That(duplicate.Predication, Is.Not.Null);
        Assert.That(duplicate.Predication, Is.TypeOf<SinglePredicationXml>());

        Assert.That(duplicate.Times, Is.Not.Null);
        Assert.That(duplicate.Times, Is.EqualTo("@myVar"));

        Assert.That(duplicate.Outputs, Is.Not.Null);
        Assert.That(duplicate.Outputs.Count, Is.EqualTo(6));
        Assert.That(duplicate.Outputs[0].Identifier.Label, Is.EqualTo("[myIndex]"));
        Assert.That(duplicate.Outputs[0].Class, Is.EqualTo(OutputClass.Index));
        Assert.That(duplicate.Outputs[1].Identifier.Label, Is.EqualTo("[myTotal]"));
        Assert.That(duplicate.Outputs[1].Class, Is.EqualTo(OutputClass.Total));
        Assert.That(duplicate.Outputs[2].Identifier.Label, Is.EqualTo("[myOriginal]"));
        Assert.That(duplicate.Outputs[2].Class, Is.EqualTo(OutputClass.IsOriginal));
        Assert.That(duplicate.Outputs[3].Identifier.Label, Is.EqualTo("[myDuplicable]"));
        Assert.That(duplicate.Outputs[3].Class, Is.EqualTo(OutputClass.IsDuplicable));
        Assert.That(duplicate.Outputs[4].Identifier.Label, Is.EqualTo("[myValue]"));
        Assert.That(duplicate.Outputs[4].Class, Is.EqualTo(OutputClass.Script));
        Assert.That(duplicate.Outputs[4].Script, Is.Not.Null);
        Assert.That(duplicate.Outputs[4].Script.Language, Is.EqualTo(LanguageType.NCalc));
        Assert.That(duplicate.Outputs[4].Script.Code, Does.Contain("[myValue] / [myTotal] * ([myIndex] + 1)"));
        Assert.That(duplicate.Outputs[5].Class, Is.EqualTo(OutputClass.Static));
        Assert.That(duplicate.Outputs[5].Value, Is.EqualTo("Monthly"));
    }

    [Test]
    public void Deserialize_SampleFile_EmptyResultSet()
    {
        int testNr = 20;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Empty, Is.Not.Null);
        Assert.That(rs.Empty, Is.TypeOf<EmptyResultSetXml>());
        var empty = rs.Empty!;

        Assert.That(empty.ColumnCount, Is.EqualTo("4"));
        Assert.That(empty.Columns, Has.Count.EqualTo(2));

        Assert.That(empty.Columns.Any(x => x.Identifier.Label == "[myFirstColumn]"));
        Assert.That(empty.Columns.Any(x => x.Identifier.Label == "[mySecondColumn]"));
    }

    [Test]
    public void Deserialize_SampleFile_IfUnavailable()
    {
        int testNr = 21;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.IfUnavailable, Is.Not.Null);
        Assert.That(rs.IfUnavailable!.ResultSet, Is.Not.Null);
        Assert.That(rs.IfUnavailable.ResultSet.Empty, Is.Not.Null);
    }

    [Test]
    public void Deserialize_SampleFile_Iteration()
    {
        int testNr = 22;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        // Check the properties of the object.
        Assert.That(ts.Tests[testNr].Systems[0], Is.AssignableTo<ResultSetSystemXml>());
        var rs = (ResultSetSystemXml)ts.Tests[testNr].Systems[0];

        Assert.That(rs.Iteration, Is.Not.Null);
        Assert.That(rs.Iteration!.Sequence, Is.Not.Null);
        Assert.That(rs.NestedResultSet, Is.Not.Null);
    }

    [Test]
    public void Serialize_FileAndParser_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml()
            {
                Path = "myFile.csv",
                Parser = new ParserXml()
                {
                    Name = "myParser",
                }
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>myFile.csv</path>"));
        Assert.That(xml, Does.Contain("<parser name=\"myParser\" />"));
        Assert.That(xml, Does.Contain("</file>"));
    }

    [Test]
    public void Serialize_InlineFileAndParser_Correct()
    {
        var root = new ResultSetSystemXml()
        {
#pragma warning disable 0618
            FilePath = "myFile.csv!myParser",
#pragma warning restore 0618
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>myFile.csv</path>"));
        Assert.That(xml, Does.Contain("<parser name=\"myParser\" />"));
        Assert.That(xml, Does.Contain("</file>"));
    }

    [Test]
    public void Serialize_InlineFileWithoutParser_Correct()
    {
        var root = new ResultSetSystemXml()
        {
#pragma warning disable 0618
            FilePath = "myFile.csv",
#pragma warning restore 0618
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<file>"));
        Assert.That(xml, Does.Contain("<path>myFile.csv</path>"));
        Assert.That(xml, Does.Not.Contain("<parser"));
        Assert.That(xml, Does.Contain("</file>"));
    }

    [Test]
    public void Serialize_Renaming_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml() { Path = @"C:\Temp\foo.txt" },
            Alterations =
            [
                new RenamingXml()
                    { Identifier= new ColumnOrdinalIdentifier(5), NewName = "myNewName" }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<rename"));
        Assert.That(xml, Does.Contain("#5"));
        Assert.That(xml, Does.Contain("myNewName"));
    }

    [Test]
    [TestCase(typeof(CountRowsXml), "count")]
    public void Serialize_CountAggregation_Correct(Type aggregationType, string serialization)
    {
        var root = new SummarizeXml()
        {
            Aggregation = (AggregationXml)(Activator.CreateInstance(aggregationType) ?? throw new NullReferenceException())
        };
        root.Aggregation.ColumnType = ColumnType.DateTime;

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain($"<{serialization}"));
        Assert.That(xml, Does.Contain("dateTime"));
    }

    [Test]
    [TestCase(typeof(SumXml), "sum")]
    [TestCase(typeof(AverageXml), "average")]
    [TestCase(typeof(MaxXml), "max")]
    [TestCase(typeof(MinXml), "min")]
    public void Serialize_SimpleAggregation_Correct(Type aggregationType, string serialization)
    {
        var root = new SummarizeXml()
        {
            Aggregation = (ColumnAggregationXml)(Activator.CreateInstance(aggregationType) ?? throw new NullReferenceException())
        };
        root.Aggregation!.ColumnType = ColumnType.DateTime;
        ((ColumnAggregationXml)root.Aggregation).Identifier = new ColumnOrdinalIdentifier(2);

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain($"<{serialization}"));
        Assert.That(xml, Does.Contain("dateTime"));
    }

    [Test]
    public void Serialize_Concatenation_Correct()
    {
        var root = new SummarizeXml()
        {
            Aggregation = new ConcatenationXml() { Separator="+" }
        };
        root.Aggregation.ColumnType = ColumnType.Text;
        ((ColumnAggregationXml)root.Aggregation).Identifier = new ColumnOrdinalIdentifier(2);

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain($"<concatenation"));
        Assert.That(xml, Does.Contain("text"));
        Assert.That(xml, Does.Contain("separator=\"+\""));
    }

    [Test]
    public void Serialize_Unstack_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new UnstackXml()
                {
                    Header = new HeaderXml()
                    {
                        Column = new ColumnDefinitionLightXml() { Identifier= new ColumnOrdinalIdentifier(2), Type= ColumnType.Text },
                        EnforcedValues =
                        [
                            "Alpha", "Beta"
                        ]
                    },
                    GroupBy = new GroupByXml()
                    {
                        Columns =
                        [
                            new ColumnDefinitionLightXml() { Identifier= new ColumnOrdinalIdentifier(0), Type= ColumnType.Numeric },
                            new ColumnDefinitionLightXml() { Identifier= new ColumnOrdinalIdentifier(1), Type= ColumnType.DateTime }
                        ]
                    },
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<unstack>"));
        Assert.That(xml, Does.Contain("<header>"));
        Assert.That(xml, Does.Contain("<column identifier=\"#2\" />"));
        Assert.That(xml, Does.Contain("<group-by>"));
        Assert.That(xml, Does.Contain("<column identifier=\"#0\" type=\"numeric\" />"));
        Assert.That(xml, Does.Contain("<column identifier=\"#1\" type=\"dateTime\" />"));
        Assert.That(xml, Does.Contain("<enforced-value>"));
        Assert.That(xml, Does.Contain(">Alpha<"));
        Assert.That(xml, Does.Contain(">Beta<"));
    }

    [Test]
    public void Serialize_Project_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new ProjectXml()
                {
                    Columns =
                    [
                        new ColumnDefinitionLightXml() { Identifier = new ColumnOrdinalIdentifier(2) },
                        new ColumnDefinitionLightXml() { Identifier = new ColumnNameIdentifier("foo") },
                    ]
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<project>"));
        Assert.That(xml, Does.Contain("<column identifier=\"#2\" />"));
        Assert.That(xml, Does.Contain("<column identifier=\"[foo]\" />"));
    }

    [Test]
    public void Serialize_ProjectAway_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new ProjectAwayXml()
                {
                    Columns =
                    [
                        new ColumnDefinitionLightXml() { Identifier = new ColumnOrdinalIdentifier(2) },
                        new ColumnDefinitionLightXml() { Identifier = new ColumnNameIdentifier("foo") },
                    ]
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<project-away>"));
        Assert.That(xml, Does.Contain("<column identifier=\"#2\" />"));
        Assert.That(xml, Does.Contain("<column identifier=\"[foo]\" />"));
    }

    [Test]
    public void Serialize_LookupReplace_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new LookupReplaceXml()
                {
                    Missing = new NBi.Xml.Items.Alteration.Lookup.MissingXml() { Behavior= Behavior.DefaultValue, DefaultValue="(null)" },
                    Join = new JoinXml() { Usings = [new ColumnUsingXml() { Column = "#1" }] },
                    ResultSet = new ResultSetSystemXml(),
                    Replacement = new ColumnDefinitionLightXml() { Identifier = new ColumnNameIdentifier("foo") }
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<lookup-replace>"));
        Assert.That(xml, Does.Contain("<missing behavior=\"default-value\">(null)</missing>"));
        Assert.That(xml, Does.Contain("<join>"));
        Assert.That(xml, Does.Contain("<result-set"));
        Assert.That(xml, Does.Contain("<replacement identifier=\"[foo]\" />"));
    }

    [Test]
    public void Serialize_LookupReplaceDefaultMissing_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new LookupReplaceXml()
                {
                    Missing = new MissingXml() { Behavior= Behavior.Failure },
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<lookup-replace"));
        Assert.That(xml, Does.Not.Contain("<missing"));
    }

    [Test]
    public void Serialize_Merge_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Alterations =
            [
                new MergeXml()
                {
                    ResultSet = new ResultSetSystemXml()
                    {
                        Sequence = new SequenceXml() { Items = ["A", "B"] },
                    }
                }
            ]
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<merge"));
        Assert.That(xml, Does.Contain("<result-set"));
        Assert.That(xml, Does.Contain("<sequence"));
        Assert.That(xml, Does.Contain("<item>A</item>"));
        Assert.That(xml, Does.Contain("<item>B</item>"));
    }

    [Test]
    public void Serialize_Sequence_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Sequence = new SequenceXml()
            {
                Items = ["A", "B"]
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<sequence"));
        Assert.That(xml, Does.Contain("<item>A</item>"));
        Assert.That(xml, Does.Contain("<item>B</item>"));
    }

    [Test]
    public void Serialize_EmptyWithoutColumnCount_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Empty = new EmptyResultSetXml()
            {
                Columns =
                [
                    new ColumnDefinitionLightXml {Identifier = new ColumnNameIdentifier("myFirstColumn")},
                    new ColumnDefinitionLightXml {Identifier = new ColumnNameIdentifier("mySecondColumn")}
                ]
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<empty>"));
        Assert.That(xml, Does.Contain("<column identifier=\"[myFirstColumn]\" />"));
        Assert.That(xml, Does.Contain("<column identifier=\"[mySecondColumn]\" />"));
        Assert.That(xml, Does.Not.Contain("column-count"));
    }

    [Test]
    public void Serialize_EmptyWithoutColumns_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            Empty = new EmptyResultSetXml { ColumnCount = "4" }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<empty"));
        Assert.That(xml, Does.Contain("column-count=\"4\""));
        Assert.That(xml, Does.Not.Contain("<column"));
    }


    [Test]
    public void Serialize_IfUnavailable_Correct()
    {
        var root = new ResultSetSystemXml()
        {
            IfUnavailable = new IfUnavailableXml
                { ResultSet = new ResultSetSystemXml { Empty = new EmptyResultSetXml { ColumnCount = "2" } } }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<if-unavailable"));
        Assert.That(xml, Does.Contain("<result-set"));
        Assert.That(xml, Does.Contain("<empty"));
    }

    [Test]
    public void Serialize_NoIteration_IterationNotDisplayed()
    {
        var root = new ResultSetSystemXml()
        {
            File = new FileXml() { Path = @"C:\Temp\foo.txt" },
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Not.Contain("<iteration"));
    }

    [Test]
    public void Serialize_Iteration_IterationAndNestedResultSetNotDisplayed()
    {
        var root = new ResultSetSystemXml()
        {
            Iteration = new IterationXml() 
            { 
                Sequence = new SequenceXml()
                { 
                    Name="month", Type=ColumnType.DateTime
                    , SentinelLoop = new SentinelLoopXml() { Seed="2020-01-01", Terminal="2020-03-01", Step="1 month" }
                }
            },
            NestedResultSet = new ResultSetSystemXml() 
            {
                File = new FileXml() { Path = @"Data_{@month:yyyy}_{@month:MM}.csv" },
            }
        };

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root);
        Assert.That(xml, Does.Contain("<iteration"));
        Assert.That(xml, Does.Contain("<sequence"));
        Assert.That(xml, Does.Contain("<loop-sentinel"));
        Assert.That(xml, Does.Contain("<result-set"));
        Assert.That(xml, Does.Contain("<file"));
    }
}
