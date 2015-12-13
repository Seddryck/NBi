using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Suite;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Suite
    {
        readonly static Parser<ISuiteAction> GenerateParser =
        (
                from generate in Keyword.Generate
                from grouping in Parse.IgnoreCase("grouping").Token().Return(true).XOr(Parse.Return(false))
                select new GenerateSuiteAction(grouping)
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
                select new IncludeSuiteAction(filename)
        );

        readonly static Parser<ISuiteAction> AddRangeParser =
        (
                from addrange in Keyword.AddRange
                from file in Keyword.File
                from filename in Grammar.QuotedTextual.Token()
                select new AddRangeSuiteAction(filename)
        );

        public readonly static Parser<IAction> Parser =
        (
                from load in Keyword.Suite
                from text in GenerateParser.Or(SaveParser).Or(IncludeParser).Or(AddRangeParser)
                select text
        );
    }
}
