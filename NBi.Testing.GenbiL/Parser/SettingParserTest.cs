using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class SettingParserTest
{
    [Test]
    public void SentenceParser_DefaultAssert_ValidSentence()
    {
        var input = "setting default assert connectionString 'youyou';";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<DefaultAction>());
        Assert.That(((DefaultAction)result).DefaultType, Is.EqualTo(DefaultType.Assert));
        Assert.That(((DefaultAction)result).Value, Is.EqualTo("youyou"));
    }

    [Test]
    public void SentenceParser_DefaultSystemUnderTest_ValidSentence()
    {
        var input = "setting default sut connectionString 'youyou';";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<DefaultAction>());
        Assert.That(((DefaultAction)result).DefaultType, Is.EqualTo(DefaultType.SystemUnderTest));
        Assert.That(((DefaultAction)result).Value, Is.EqualTo("youyou"));
    }

    [Test]
    public void SentenceParser_Reference_ValidSentence()
    {
        var input = "setting reference 'no way' connectionString 'youyou';";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ReferenceAction>());
        Assert.That(((ReferenceAction)result).Name, Is.EqualTo("no way"));
        Assert.That(((ReferenceAction)result).Value, Is.EqualTo("youyou"));
    }

    [Test]
    public void SentenceParser_ParallelizeQueriesOn_ValidSentence()
    {
        var input = "setting set 'parallelize-queries' on;";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ParallelizeQueriesAction>());
        Assert.That(((ParallelizeQueriesAction)result).Value, Is.True);
    }

    [Test]
    public void SentenceParser_ParallelizeQueriesOff_ValidSentence()
    {
        var input = "setting set 'parallelize-queries' off;";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ParallelizeQueriesAction>());
        Assert.That(((ParallelizeQueriesAction)result).Value, Is.False);
    }

    [Test]
    public void SentenceParser_IncludeSetting_ValidSentence()
    {
        var input = "setting include file 'myfile.nbiset';";
        var result = Setting.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeSettingAction>());
        Assert.That(((IncludeSettingAction)result).Filename, Is.EqualTo("myfile.nbiset"));
    }
}
