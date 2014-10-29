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
                from filename in Grammar.QuotedTextual.Token()
                select new SaveSuiteAction(filename)
        );

        public readonly static Parser<IAction> Parser =
        (
                from load in Keyword.Suite
                from text in GenerateParser.Or(SaveParser)
                select text
        );
    }
}
