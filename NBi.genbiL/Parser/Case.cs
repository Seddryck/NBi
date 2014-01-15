using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Case;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Case
    {
        public static readonly Parser<LoadType> LoadTypeParser =
            Parse.IgnoreCase("file").Return(LoadType.File)
                .Token();

        public static readonly Parser<AxisType> AxisTypeParser =
            Parse.IgnoreCase("column").Return(AxisType.Column)
                .Token();

        readonly static Parser<ICaseAction> CaseLoadParser =
        (
                from load in Keyword.Load
                from loadType in LoadTypeParser
                from filename in Grammar.QuotedTextual
                select new LoadCaseAction(filename)
        );


        readonly static Parser<ICaseAction> CaseRemoveParser =
        (
                from remove in Keyword.Remove
                from axisType in AxisTypeParser
                from variableName in Grammar.QuotedTextual
                select new RemoveCaseAction(variableName)
        );

        readonly static Parser<ICaseAction> CaseRenameParser =
        (
                from remove in Keyword.Rename
                from axisType in Parse.IgnoreCase("Column").Token()
                from oldVariableName in Grammar.QuotedTextual
                from intoKeyword in Keyword.Into
                from newVariableName in Grammar.QuotedTextual
                select new RenameCaseAction(oldVariableName, newVariableName)
        );

        public readonly static Parser<IAction> Parser =
        (
                from load in Keyword.Case
                from text in CaseLoadParser.Or(CaseRemoveParser).Or(CaseRenameParser)
                select text
        );
    }
}
