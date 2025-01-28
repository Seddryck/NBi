using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Suite;
using Sprache;

namespace NBi.GenbiL.Parser;

class Suite
{
    readonly static Parser<ISuiteAction> GenerateParser =
    (
            from generate in Keyword.Generate
            from grouping in Parse.IgnoreCase("grouping").Token().Return(true).XOr(Parse.Return(false))
            select new GenerateTestSuiteAction(grouping)
    );

    readonly static Parser<ISuiteAction> GenerateTestGroupByParser =
    (
            from generate in Keyword.Generate
            from test in (Parse.IgnoreCase("tests").Or(Parse.IgnoreCase("test"))).Token().Optional()
            from groupby in Parse.IgnoreCase("group by")
            from groupPattern in Grammar.QuotedTextual.Token()
            select new GenerateTestGroupBySuiteAction(groupPattern)
    );

    readonly static Parser<ISuiteAction> GenerateSetupGroupByParser =
    (
            from generate in Keyword.Generate
            from setup in (Parse.IgnoreCase("setups").Or(Parse.IgnoreCase("setup"))).Token()
            from groupby in Parse.IgnoreCase("group by")
            from groupPattern in Grammar.QuotedTextual.Token()
            select new GenerateSetupGroupBySuiteAction(groupPattern)
    );

    readonly static Parser<ISuiteAction> GenerateCleanupGroupByParser =
    (
            from generate in Keyword.Generate
            from setup in (Parse.IgnoreCase("cleanups").Or(Parse.IgnoreCase("cleanup"))).Token()
            from groupby in Parse.IgnoreCase("group by")
            from groupPattern in Grammar.QuotedTextual.Token()
            select new GenerateCleanupGroupBySuiteAction(groupPattern)
    );

    readonly static Parser<ISuiteAction> SaveParser =
    (
            from save in Keyword.Save
            from @as in Keyword.As.Optional()
            from filename in Grammar.QuotedTextual.Token()
            select new SaveSuiteAction(filename)
    );

    readonly static Parser<ISuiteAction> IncludeParser =
    (
            from include in (Keyword.Add).Or(Keyword.Include)
            from file in Keyword.File
            from filename in Grammar.QuotedTextual.Token()
            from groupName in GroupNameParser.Optional()
            select new IncludeSuiteAction(filename, groupName.GetOrElse("."))
    );

    readonly static Parser<string> GroupNameParser =
    (
        from @into in (Keyword.Into).Or(Keyword.To)
        from @group in Keyword.Group
        from groupName in Grammar.QuotedTextual.Token()
        select groupName
    );

    readonly static Parser<ISuiteAction> AddRangeParser =
    (
            from addrange in Keyword.AddRange
            from file in Keyword.File
            from filename in Grammar.QuotedTextual.Token()
            from groupName in GroupNameParser.Optional()
            select new AddRangeSuiteAction(filename, groupName.GetOrElse("."))
    );

    public readonly static Parser<IAction> Parser =
    (
            from suite in Keyword.Suite
            from text in GenerateSetupGroupByParser.Or(GenerateCleanupGroupByParser).Or(GenerateTestGroupByParser).Or(GenerateParser).Or(SaveParser).Or(IncludeParser).Or(AddRangeParser)
            select text
    );
}
