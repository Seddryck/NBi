using System;
using System.Linq;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class SuiteParserTest
{
    [Test]
    public void SentenceParser_SuiteGenerate_ValidGenerateSuiteAction()
    {
        var input = "suite generate;";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateTestSuiteAction>());
        Assert.That(((GenerateTestSuiteAction)result).Grouping, Is.False);
    }

    [Test]
    public void SentenceParser_SuiteGenerateGrouping_ValidGenerateSuiteAction()
    {
        var input = "suite generate grouping;";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateTestSuiteAction>());
        Assert.That(((GenerateTestSuiteAction)result).Grouping, Is.True);
    }

    [Test]
    public void SentenceParser_SuiteGenerateGroupBy_ValidGenerateGroupBySuiteAction()
    {
        var input = "suite generate group by '$group$|$subgroup$';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateTestGroupBySuiteAction>());
        Assert.That(((GenerateTestGroupBySuiteAction)result).Grouping, Is.False);
        Assert.That(((GenerateTestGroupBySuiteAction)result).GroupByPattern, Is.EqualTo("$group$|$subgroup$"));
    }

    [Test]
    public void SentenceParser_SuiteGenerateTestsGroupBy_ValidGenerateGroupBySuiteAction()
    {
        var input = "suite generate tests group by '$group$|$subgroup$';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateTestGroupBySuiteAction>());
        Assert.That(((GenerateTestGroupBySuiteAction)result).Grouping, Is.False);
        Assert.That(((GenerateTestGroupBySuiteAction)result).GroupByPattern, Is.EqualTo("$group$|$subgroup$"));
    }

    [Test]
    public void SentenceParser_SuiteGenerateSetupTestsGroupBy_ValidGenerateGroupBySuiteAction()
    {
        var input = "suite generate setups group by '$group$|$subgroup$';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateSetupGroupBySuiteAction>());
        Assert.That(((GenerateSetupGroupBySuiteAction)result).Grouping, Is.False);
        Assert.That(((GenerateSetupGroupBySuiteAction)result).GroupByPattern, Is.EqualTo("$group$|$subgroup$"));
    }

    [Test]
    public void SentenceParser_SuiteGenerateCleanupTestsGroupBy_ValidGenerateGroupBySuiteAction()
    {
        var input = "suite generate cleanups group by '$group$|$subgroup$';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<GenerateCleanupGroupBySuiteAction>());
        Assert.That(((GenerateCleanupGroupBySuiteAction)result).Grouping, Is.False);
        Assert.That(((GenerateCleanupGroupBySuiteAction)result).GroupByPattern, Is.EqualTo("$group$|$subgroup$"));
    }

    [Test]
    public void SentenceParser_SuiteSaveString_ValidSaveSuiteAction()
    {
        var input = "suite save 'filename.nbits';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<SaveSuiteAction>());
        Assert.That(((SaveSuiteAction)result).Filename, Is.EqualTo("filename.nbits"));
    }

    [Test]
    public void SentenceParser_SuiteSaveAsString_ValidSaveSuiteAction()
    {
        var input = "suite save as 'filename.nbits';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<SaveSuiteAction>());
        Assert.That(((SaveSuiteAction)result).Filename, Is.EqualTo("filename.nbits"));
    }

    [Test]
    public void SentenceParser_SuiteIncludeFileString_ValidIncludeSuiteAction()
    {
        var input = "suite include file 'filename.nbitx';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeSuiteAction>());
        Assert.That(((IncludeSuiteAction)result).Filename, Is.EqualTo("filename.nbitx"));
        Assert.That(((IncludeSuiteAction)result).GroupPath, Is.EqualTo("."));
    }

    [Test]
    public void SentenceParser_SuiteIncludeIntoGroupString_ValidIncludeSuiteAction()
    {
        var input = "suite include file 'filename.nbitx' into group 'group|subgroup';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeSuiteAction>());
        Assert.That(((IncludeSuiteAction)result).Filename, Is.EqualTo("filename.nbitx"));
        Assert.That(((IncludeSuiteAction)result).GroupPath, Is.EqualTo("group|subgroup"));
    }

    [Test]
    public void SentenceParser_SuiteAddFileString_ValidIncludeSuiteAction()
    {
        var input = "suite add file 'filename.nbitx';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeSuiteAction>());
        Assert.That(((IncludeSuiteAction)result).Filename, Is.EqualTo("filename.nbitx"));
    }

    [Test]
    public void SentenceParser_SuiteAddToGroupString_ValidIncludeSuiteAction()
    {
        var input = "suite add file 'filename.nbitx' to group 'group|subgroup';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IncludeSuiteAction>());
        Assert.That(((IncludeSuiteAction)result).Filename, Is.EqualTo("filename.nbitx"));
        Assert.That(((IncludeSuiteAction)result).GroupPath, Is.EqualTo("group|subgroup"));
    }

    [Test]
    public void SentenceParser_SuiteAddRangeFileString_ValidAddRangeSuiteAction()
    {
        var input = "suite addrange file 'filename.nbits';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<AddRangeSuiteAction>());
        Assert.That(((AddRangeSuiteAction)result).Filename, Is.EqualTo("filename.nbits"));
    }

    [Test]
    public void SentenceParser_SuiteAddRangeFileToGroupString_ValidAddRangeSuiteAction()
    {
        var input = "suite addrange file 'filename.nbits' to group 'group|subgroup';";
        var result = Suite.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<AddRangeSuiteAction>());
        Assert.That(((AddRangeSuiteAction)result).Filename, Is.EqualTo("filename.nbits"));
        Assert.That(((AddRangeSuiteAction)result).GroupPath, Is.EqualTo("group|subgroup"));
    }
}
