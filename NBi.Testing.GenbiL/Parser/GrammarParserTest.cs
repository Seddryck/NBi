using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;
using NBi.GenbiL.Parser.Valuable;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class GrammarParserTest
{
    [Test]
    public void ExtendedQuotedTextual_QuotedText_ReturnValue()
    {
        var input = "'alpha'";
        var result = Grammar.ExtendedQuotedTextual.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("alpha"));
    }

    [Test]
    public void ExtendedQuotedTextual_Empty_ReturnValue()
    {
        var input = "empty";
        var result = Grammar.ExtendedQuotedTextual.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void ExtendedQuotedTextual_None_ReturnValue()
    {
        var input = "none";
        var result = Grammar.ExtendedQuotedTextual.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("(none)"));
    }

    [Test]
    public void ExtendedQuotedRecordSequence_ValueNoneValueEmpty_ReturnFourResults()
    {
        var input = "'alpha', none,'beta',empty";
        var result = Grammar.ExtendedQuotedRecordSequence.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Member("(none)"));
        Assert.That(result, Has.Member(""));
        Assert.That(result, Has.Member("alpha"));
        Assert.That(result, Has.Member("beta"));
        Assert.That(result.Count(), Is.EqualTo(4));
    }

    [Test]
    public void Valuable_ColumnName_ReturnColumn()
    {
        var input = "column 'alpha'";
        var result = Grammar.Valuables.Parse(input);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.ElementAt(0), Is.TypeOf<Column>());
        Assert.That(((Column)(result.ElementAt(0))).Name, Is.EqualTo("alpha"));
    }

    [Test]
    public void Valuable_ColumnNameWithPluralAtColumn_ReturnColumn()
    {
        var input = "columns 'alpha'";
        var result = Grammar.Valuables.Parse(input);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.ElementAt(0), Is.TypeOf<Column>());
        Assert.That(((Column)(result.ElementAt(0))).Name, Is.EqualTo("alpha"));
    }

    [Test]
    public void Valuable_ThreeColumnNames_ReturnColumns()
    {
        var input = "column 'alpha','beta', 'gamma'";
        var result = Grammar.Valuables.Parse(input);

        

        Assert.That(result.Count(), Is.EqualTo(3));
        foreach (var item in result)
            Assert.That(item, Is.TypeOf<Column>());

        var names = result.Select(x => ((Column)x).Name);
        Assert.That(names, Is.EquivalentTo(new[] { "alpha", "beta", "gamma" }));
    }

    [Test]
    public void Valuable_ValueName_ReturnValue()
    {
        var input = "Value 'alpha'";
        var result = Grammar.Valuables.Parse(input);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.ElementAt(0), Is.TypeOf<Value>());
        Assert.That(((Value)(result.ElementAt(0))).Text, Is.EqualTo("alpha"));
    }

    [Test]
    public void Valuable_ValueNameWithPluralAtValue_ReturnValue()
    {
        var input = "Values 'alpha'";
        var result = Grammar.Valuables.Parse(input);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.ElementAt(0), Is.TypeOf<Value>());
        Assert.That(((Value)(result.ElementAt(0))).Text, Is.EqualTo("alpha"));
    }

    [Test]
    public void Valuable_ThreeValueNames_ReturnValues()
    {
        var input = "vAlues 'alpha','beta', 'gamma'";
        var result = Grammar.Valuables.Parse(input);


        Assert.That(result.Count(), Is.EqualTo(3));
        foreach (var item in result)
            Assert.That(item, Is.TypeOf<Value>());

        var names = result.Select(x => ((Value)x).Text);
        Assert.That(names, Is.EquivalentTo(new[] { "alpha", "beta", "gamma" }));
    }

}
