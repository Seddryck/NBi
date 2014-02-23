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

        readonly static Parser<ICaseAction> CaseMoveParser =
        (
                from move in Keyword.Move
                from axisType in Parse.IgnoreCase("Column").Token()
                from variableName in Grammar.QuotedTextual
                from intoKeyword in Keyword.To
                from relativePosition in Parse.IgnoreCase("Left").Return(-1).Or(Parse.IgnoreCase("Left").Return(1)).Token()
                select new MoveCaseAction(variableName, relativePosition)
        );

        public readonly static Parser<IAction> Parser =
        (
                from @case in Keyword.Case
                from action in CaseLoadParser.Or(CaseRemoveParser).Or(CaseRenameParser).Or(CaseMoveParser)
                select action
        );
    }
}
