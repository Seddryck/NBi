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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.Diagnostics;
using System;
using NBi.Xml.SerializationOption;
using NBi.Xml.Variables;
using NBi.Core.Transformation;
#endregion

namespace NBi.Xml.Testing.Unit.Constraints;

[TestFixture]
public class AllRowsXmlTest : BaseXmlTest
{
    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyAllRows()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();

        Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<AllRowsXml>());
        Assert.That(ts.Tests[testNr].Constraints[0].Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyFormulaComparer()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var comparison = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)comparison.Operand!).Name, Is.EqualTo("ModDepId"));
        Assert.That(comparison.ColumnType, Is.EqualTo(ColumnType.Numeric));

        Assert.That(comparison.Predicate, Is.TypeOf<MoreThanXml>());
        var moreThan = (MoreThanXml)comparison.Predicate!;
        Assert.That(moreThan.Reference, Is.EqualTo("10"));
        Assert.That(moreThan.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyVariables()
    {
        int testNr = 0;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
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
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));
        Assert.That(predicate.Predicate, Is.TypeOf<EmptyXml>());

        var emptyPredicate = (EmptyXml)predicate.Predicate!;
        Assert.That(emptyPredicate.OrNull, Is.True);
        Assert.That(emptyPredicate.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyAliases()
    {
        int testNr = 1;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
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
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

        Assert.That(predicate.Predicate, Is.TypeOf<StartsWithXml>());
        var cpr = (StartsWithXml)predicate.Predicate!;
        Assert.That(cpr.IgnoreCase, Is.False);
        Assert.That(cpr.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyEndsWithComparer()
    {
        int testNr = 3;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));
        Assert.That(predicate.Predicate, Is.TypeOf<EndsWithXml>());

        var cpr = (EndsWithXml)predicate.Predicate!;
        Assert.That(cpr.IgnoreCase, Is.False);
        Assert.That(cpr.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyContainsComparer()
    {
        int testNr = 4;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

        Assert.That(predicate.Predicate, Is.TypeOf<ContainsXml>());
        var cpr = (ContainsXml)predicate.Predicate!;
        Assert.That(cpr.IgnoreCase, Is.True);
        Assert.That(cpr.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyMatchesRegexComparer()
    {
        int testNr = 5;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

        Assert.That(predicate.Predicate, Is.TypeOf<MatchesRegexXml>());
        Assert.That(predicate.Predicate!.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyLowerCaseComparer()
    {
        int testNr = 6;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

        Assert.That(predicate.Predicate, Is.TypeOf<LowerCaseXml>());
        Assert.That(predicate.Predicate!.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyUpperCaseComparer()
    {
        int testNr = 7;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Name"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Text));

        Assert.That(predicate.Predicate, Is.TypeOf<UpperCaseXml>());
        Assert.That(predicate.Predicate!.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyWithinRangeComparer()
    {
        int testNr = 8;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!  ;
        var predicate = allRows.Predication;

        Assert.That(((ColumnNameIdentifier)predicate.Operand!).Name, Is.EqualTo("Value"));
        Assert.That(predicate.ColumnType, Is.EqualTo(ColumnType.Numeric));

        Assert.That(predicate.Predicate, Is.TypeOf<WithinRangeXml>());
        var cpr = (WithinRangeXml)predicate.Predicate!;
        Assert.That(cpr.Reference, Is.EqualTo("[10;30]"));
        Assert.That(cpr.Not, Is.False);
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyWithinListComparer()
    {
        int testNr = 9;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(predicate.Predicate, Is.AssignableTo<AnyOfXml>());
        var cpr = (AnyOfXml)predicate.Predicate!;
        Assert.That(cpr.References, Has.Count.EqualTo(3));
    }


    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyAnyOfComparer()
    {
        int testNr = 10;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;
        var predicate = allRows.Predication;

        Assert.That(predicate.Predicate, Is.AssignableTo<AnyOfXml>());
        var cpr = (AnyOfXml)predicate.Predicate!;
        Assert.That(cpr.References, Has.Count.EqualTo(3));
    }

    [Test]
    public void Deserialize_SampleFile_ReadCorrectlyMultipleExpressions()
    {
        int testNr = 11;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;

        Assert.That(allRows.Expressions, Is.AssignableTo<IEnumerable<ExpressionXml>>());
        Assert.That(allRows.Expressions, Has.Count.EqualTo(2));
    }

    [Test]
    public void Deserialize_SampleFile_ScriptWithinExpressions()
    {
        int testNr = 12;

        // Create an instance of the XmlSerializer specifying type and namespace.
        var ts = DeserializeSample();
        var allRows = (AllRowsXml)ts.Tests[testNr].Constraints[0]!;

        Assert.That(allRows.Expressions, Is.AssignableTo<IEnumerable<ExpressionXml>>());
        Assert.That(allRows.Expressions, Has.Count.EqualTo(1));
        Assert.That(allRows.Expressions.ElementAt(0).Script, Is.Not.Null);
        var script = allRows.Expressions.ElementAt(0).Script!;
        Assert.That(script.Language, Is.EqualTo(LanguageType.Native));
        Assert.That(script.Code, Does.Contain("DeptId | numeric-to-integer"));
    }

    [Test]
    public void Serialize_AllRowsXml_OnlyAliasNoVariable()
    {
        var allRowsXml = new AllRowsXml
#pragma warning disable 0618
        {
            InternalAliasesOld =
            [
                new AliasXml() {Column = 1, Name="Col1"},
                new AliasXml() {Column = 0, Name="Col2"}
            ],
            Predication = new SinglePredicationXml()
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

        Assert.That(content, Does.Contain("alias"));
        Assert.That(content, Does.Not.Contain("variable"));
    }

    [Test]
    public void Serialize_AllRowsXml_AnyOfXml()
    {
        var allRowsXml = new AllRowsXml
        {
            Predication = new SinglePredicationXml()
            {
                Predicate = new AnyOfXml()
                {
                    References = ["first", "second"]
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

        Assert.That(content, Does.Contain("any-of"));
        Assert.That(content, Does.Contain("item"));
        Assert.That(content, Does.Contain("first"));
        Assert.That(content, Does.Contain("second"));
    }

    [Test]
    public void Serialize_ExecutionXml_NoColumnOrdinal()
    {
        var allRowsXml = new AllRowsXml
        {
            Expressions =
            [
                new ExpressionXml()
                {
                    Value = "a + b = c",
                    Type = ColumnType.Boolean,
                    Name = "calculate"
                }
            ]
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

        Assert.That(content, Does.Contain("expression"));
        Assert.That(content, Does.Contain("type"));
        Assert.That(content, Does.Contain("name"));
        Assert.That(content, Does.Contain(">a + b = c<"));
        Assert.That(content, Does.Not.Contain("column-type"));
        Assert.That(content, Does.Not.Contain("column-index"));
        Assert.That(content, Does.Not.Contain("tolerance"));
    }

    [Test]
    public void Serialize_ExecutionAndAliasesXml_AliasesBeforeExecution()
    {
        var allRowsXml = new AllRowsXml
        {
            Expressions =
            [
                new ExpressionXml()
                {
                    Value = "a + b - c",
                    Type = ColumnType.Numeric,
                    Name = "calculate"
                }
            ],

            InternalAliases =
            [
                new AliasXml()
                {
                    Column = 0,
                    Name = "a"
                },
                new AliasXml()
                {
                    Column = 1,
                    Name = "b"
                },
                new AliasXml()
                {
                    Column = 2,
                    Name = "c"
                }
            ],

            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnNameIdentifier("calculate"),
                ColumnType = ColumnType.Numeric,
                Predicate = new EqualXml() { Reference = "100" }
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

        Assert.That(content, Does.Contain("<alias"));
        Assert.That(content, Does.Contain("<expression"));
        Assert.That(content, Does.Contain("<predicate"));
        Assert.That(content.LastIndexOf("<alias"), Is.LessThan(content.IndexOf("<expression")));
        Assert.That(content.LastIndexOf("<expression"), Is.LessThan(content.IndexOf("<predicate")));
    }

    [Test]
    public void Serialize_UnspecifiedExpression_NoScript()
    {
        var allRowsXml = new AllRowsXml
        {
            Expressions =
            [
                new ExpressionXml()
                {
                    Value = "a + b - c",
                    Type = ColumnType.Numeric,
                    Name = "calculate"
                }
            ],

            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnNameIdentifier("calculate"),
                ColumnType = ColumnType.Numeric,
                Predicate = new EqualXml() { Reference = "100" }
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

        Assert.That(content, Does.Contain("<expression"));
        Assert.That(content, Does.Contain("a + b - c"));
        Assert.That(content.IndexOf("a + b - c"), Is.EqualTo(content.LastIndexOf("a + b - c")));
    }

    [Test]
    public void Serialize_NCalcExpression_NoScript()
    {
        var allRowsXml = new AllRowsXml
        {
            Expressions =
            [
                new ExpressionXml()
                {
                    Type = ColumnType.Numeric,
                    Name = "calculate",
                    Script = new ScriptXml() { Code = "a + b - c", Language = LanguageType.NCalc }
                }
            ],

            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnNameIdentifier("calculate"),
                ColumnType = ColumnType.Numeric,
                Predicate = new EqualXml() { Reference = "100" }
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

        Assert.That(content, Does.Contain("<expression"));
        Assert.That(content, Does.Contain("a + b - c"));
        Assert.That(content.IndexOf("a + b - c"), Is.EqualTo(content.LastIndexOf("a + b - c")));
    }

    [Test]
    public void Serialize_NativeExpression_ScriptIsAvailable()
    {
        var allRowsXml = new AllRowsXml
        {
            Expressions =
            [
                new ExpressionXml()
                {
                    Type = ColumnType.Numeric,
                    Name = "calculate",
                    Script = new ScriptXml() { Code = "a | numeric-to-integer", Language = LanguageType.Native }
                }
            ],

            Predication = new SinglePredicationXml()
            {
                Operand = new ColumnNameIdentifier("calculate"),
                ColumnType = ColumnType.Numeric,
                Predicate = new EqualXml() { Reference = "100" }
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

        Assert.That(content, Does.Contain("<expression"));
        Assert.That(content, Does.Contain("<script"));
        Assert.That(content, Does.Contain("native"));
        Assert.That(content, Does.Contain("a | numeric-to-integer"));
        Assert.That(content.IndexOf("a | numeric-to-integer"), Is.EqualTo(content.LastIndexOf("a | numeric-to-integer")));
    }

    [Test]
    public void Serialize_MatchesRegex_WithCDATA()
    {
        var root = new SinglePredicationXml()
        {
            Predicate = new MatchesRegexXml { Reference = "<|>|&" }
        };

        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root, overrides);
        Assert.That(xml, Does.Contain("<matches-regex>"));
        Assert.That(xml, Does.Not.Contain("<ValueWrite>"));
        Assert.That(xml, Does.Contain("<![CDATA[<|>|&]]>"));
        Assert.That(xml, Does.Not.Contain("&lt;|&gt;|&amp;"));
    }

    [Test]
    public void Deserialize_MatchesRegex_WithCDATA()
    {
        var xml = "<SinglePredicationXml xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><matches-regex><![CDATA[<|>|&]]></matches-regex></SinglePredicationXml>";
        var manager = new XmlManager();
        var overrides = new ReadOnlyAttributes();
        overrides.Build();
        var objectData = manager.XmlDeserializeTo<SinglePredicationXml>(xml, overrides);
        Assert.That(objectData, Is.TypeOf<SinglePredicationXml>());
        Assert.That(objectData, Is.Not.Null);
        Assert.That(objectData.Predicate, Is.TypeOf<MatchesRegexXml>());
        var predicate = (MatchesRegexXml)objectData.Predicate!;
        Assert.That(predicate, Is.Not.Null);
        Assert.That(predicate.Reference, Is.EqualTo("<|>|&"));
    }

    [Test]
    public void Deserialize_MatchesRegex_WithoutCDATA()
    {
        var xml = "<SinglePredicationXml xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><matches-regex>&lt;|&gt;|&amp;</matches-regex></SinglePredicationXml>";
        var manager = new XmlManager();
        var overrides = new ReadOnlyAttributes();
        overrides.Build();
        var objectData = manager.XmlDeserializeTo<SinglePredicationXml>(xml, overrides);
        Assert.That(objectData, Is.TypeOf<SinglePredicationXml>());
        Assert.That(objectData, Is.Not.Null);
        Assert.That(objectData.Predicate, Is.TypeOf<MatchesRegexXml>());
        var predicate = (MatchesRegexXml)objectData.Predicate!;
        Assert.That(predicate, Is.Not.Null);
        Assert.That(predicate.Reference, Is.EqualTo("<|>|&"));
    }

    [Test]
    public void Serialize_Equal_WithoutCDATAButWithZero()
    {
        var root = new SinglePredicationXml()
        {
            Predicate = new EqualXml() { Reference = "0" }
        };

        var overrides = new WriteOnlyAttributes();
        overrides.Build();

        var manager = new XmlManager();
        var xml = manager.XmlSerializeFrom(root, overrides);
        Assert.That(xml, Does.Contain("<equal>0</equal>"));
        Assert.That(xml, Does.Not.Contain("<equal />"));
    }

    [Test]
    public void Deserialize_Equal_WithCDATA()
    {
        var xml = "<SinglePredicationXml xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><equal><![CDATA[<|>|&]]></equal></SinglePredicationXml>";
        var manager = new XmlManager();
        var overrides = new ReadOnlyAttributes();
        overrides.Build();
        var objectData = manager.XmlDeserializeTo<SinglePredicationXml>(xml, overrides);
        Assert.That(objectData, Is.TypeOf<SinglePredicationXml>());
        Assert.That(objectData, Is.Not.Null);
        Assert.That(objectData.Predicate, Is.TypeOf<EqualXml>());
        var predicate = (EqualXml)objectData.Predicate!;
        Assert.That(predicate, Is.Not.Null);
        Assert.That(predicate.Reference, Is.EqualTo("<|>|&"));
    }

    [Test]
    public void Deserialize_Equal_WithoutCDATA()
    {
        var xml = "<SinglePredicationXml xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><equal>&lt;|&gt;|&amp;</equal></SinglePredicationXml>";
        var manager = new XmlManager();
        var overrides = new ReadOnlyAttributes();
        overrides.Build();
        var objectData = manager.XmlDeserializeTo<SinglePredicationXml>(xml, overrides);
        Assert.That(objectData, Is.TypeOf<SinglePredicationXml>());
        Assert.That(objectData, Is.Not.Null);
        Assert.That(objectData.Predicate, Is.TypeOf<EqualXml>());
        var predicate = (EqualXml)objectData.Predicate!;
        Assert.That(predicate, Is.Not.Null);
        Assert.That(predicate.Reference, Is.EqualTo("<|>|&"));
    }
}
