using global::NUnit.Framework;
using global::NUnit.Framework.Interfaces;
using NBi.NUnit.Runtime.Embed;
using NBi.NUnit.Runtime.Embed.Filter;
using NBi.Core.Analysis.Member;
using MembersContainConstraint = NBi.NUnit.Member.ContainConstraint;
using StructureContainConstraint = NBi.NUnit.Structure.ContainConstraint;

namespace NBi.Framework.Testing;

[TestFixture]
[NonParallelizable]
public class NUnitRuntimeTest
{
    private const string Rows = """
        <row><cell>1</cell><cell>10</cell></row>
        <row><cell>2</cell><cell>20</cell></row>
        """;

    private const string ExpectedResultSet = "<result-set>" + Rows + "</result-set>";
    private const string LookupJoin = "<join><mapping candidate='#0' reference='#0' type='numeric'/></join>";

    [TestCase("<row-count><equal>2</equal></row-count>", TestStatus.Passed)]
    [TestCase("<row-count><less-than>3</less-than></row-count>", TestStatus.Passed)]
    [TestCase("<row-count not='true'><equal>3</equal></row-count>", TestStatus.Passed)]
    [TestCase("<row-count not='true'><equal>2</equal></row-count>", TestStatus.Failed)]
    [TestCase("<row-count><equal>3</equal></row-count>", TestStatus.Failed, "count of rows")]
    [TestCase("<unique-rows/>", TestStatus.Passed)]
    [TestCase("<all-rows><predicate operand='#0' type='numeric'><more-than>0</more-than></predicate></all-rows>", TestStatus.Passed)]
    [TestCase("<no-rows><predicate operand='#0' type='numeric'><more-than>2</more-than></predicate></no-rows>", TestStatus.Passed)]
    [TestCase("<some-rows><predicate operand='#0' type='numeric'><more-than>1</more-than></predicate></some-rows>", TestStatus.Passed)]
    [TestCase("<single-row><predicate operand='#0' type='numeric'><more-than>1</more-than></predicate></single-row>", TestStatus.Passed)]
    [TestCase("<equal-to>" + ExpectedResultSet + "</equal-to>", TestStatus.Passed)]
    [TestCase("<subset-of>" + ExpectedResultSet + "</subset-of>", TestStatus.Passed)]
    [TestCase("<superset-of>" + ExpectedResultSet + "</superset-of>", TestStatus.Passed)]
    [TestCase("<intersection-of>" + ExpectedResultSet + "</intersection-of>", TestStatus.Passed)]
    [TestCase("<lookup-exists>" + LookupJoin + ExpectedResultSet + "</lookup-exists>", TestStatus.Passed)]
    [TestCase("<lookup-exists>" + LookupJoin + "<result-set><row><cell>9</cell><cell>10</cell></row></result-set></lookup-exists>", TestStatus.Failed)]
    [TestCase("<lookup-matches>" + LookupJoin + "<inclusion><mapping candidate='#1' reference='#1' type='numeric'/></inclusion>" + ExpectedResultSet + "</lookup-matches>", TestStatus.Passed)]
    [TestCase("<row-count><filter><predicate operand='#0' type='numeric'><more-than>1</more-than></predicate></filter><equal>50%</equal></row-count>", TestStatus.Passed)]
    [TestCase("<row-count><filter><predicate operand='#0' type='numeric'><more-than>1</more-than></predicate></filter><equal>75%</equal></row-count>", TestStatus.Failed, "50%")]
    public void Execute_NbitsAssertion_ReturnsNUnitResult(string assertion, TestStatus status, string? message = null)
    {
        const string testName = "Inline assertion";
        var directory = Directory.CreateTempSubdirectory();
        try
        {
            var config = Path.Combine(directory.FullName, "suite.config");
            File.WriteAllText(config, """
                <configuration>
                  <configSections>
                    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime" />
                  </configSections>
                  <nbi testSuite="suite.nbits" />
                </configuration>
                """);
            File.WriteAllText(Path.Combine(directory.FullName, "suite.nbits"), $"""
                <testSuite name="NUnit runtime" xmlns="http://NBi/TestSuite">
                  <test name="{testName}">
                    <system-under-test><result-set>{Rows}</result-set></system-under-test>
                    <assert>{assertion}</assert>
                  </test>
                </testSuite>
                """);

            var run = new Engine().Execute(config, new NBiNameFilter(testName));
            var result = LeafResults(run).Single();
            Assert.That(result.Test.Name, Is.EqualTo(testName), result.Message);
            Assert.That(result.ResultState.Status, Is.EqualTo(status), result.Message);
            Assert.That(result.ResultState.Label, Is.Not.EqualTo("Error"), result.Message);
            if (message is not null)
                Assert.That(result.Message, Does.Contain(message));
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [TestCase("First", true)]
    [TestCase("Unknown", false)]
    public void MembersContain_ApplyTo_ComparesMemberCaptions(string expected, bool success)
    {
        var members = new MemberResult();
        members.Add(new Member("[First]", "First", 1, 0));
        var constraint = new MembersContainConstraint(expected);

        Assert.That(constraint.ApplyTo(members).IsSuccess, Is.EqualTo(success));
    }

    [TestCase("FIRST", true)]
    [TestCase("Unknown", false)]
    public void StructureContain_ApplyTo_HonorsIgnoreCase(string expected, bool success)
    {
        var constraint = new StructureContainConstraint(expected).IgnoreCase;

        Assert.That(constraint.ApplyTo(new[] { "First" }).IsSuccess, Is.EqualTo(success));
    }

    private static IEnumerable<ITestResult> LeafResults(ITestResult result)
        => result.HasChildren ? result.Children.SelectMany(LeafResults) : [result];
}
