using System;
using System.Linq;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Case;
using NBi.Service;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Case
    {
        public static readonly Parser<LoadType> LoadTypeFileParser =
            Parse.IgnoreCase("file").Return(LoadType.File)
                .Token();

        public static readonly Parser<LoadType> LoadTypeQueryParser =
            Parse.IgnoreCase("query").Return(LoadType.Query)
                .Token();

        public static readonly Parser<AxisType> AxisTypeParser =
            Parse.IgnoreCase("column").Return(AxisType.Column)
                .Token();

        readonly static Parser<ICaseAction> CaseLoadFileParser =
        (
                from load in Keyword.Load
                from loadType in LoadTypeFileParser
                from filename in Grammar.QuotedTextual
                select new LoadCaseFromFileAction(filename)
        );

        readonly static Parser<ICaseAction> CaseLoadQueryParser =
        (
                from load in Keyword.Load
                from loadType in LoadTypeQueryParser
                from filename in Grammar.QuotedTextual
                from onKeyword in Keyword.On
                from connectionString in Grammar.QuotedTextual
                select new LoadCaseFromQueryAction(filename, connectionString)
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
                from toKeyword in Keyword.To
                from relativePosition in Parse.IgnoreCase("Left").Return(-1).Or(Parse.IgnoreCase("Right").Return(1)).Token()
                select new MoveCaseAction(variableName, relativePosition)
        );

        readonly static Parser<ICaseAction> CaseFilterParser =
        (
                from filter in Keyword.Filter
                from onKeyword in Keyword.On
                from axisType in Parse.IgnoreCase("Column").Token()
                from variableName in Grammar.QuotedTextual
                from valuesKeyword in Keyword.Values
                from negation in Keyword.Not.Optional()
                from @operator in Parse.IgnoreCase("Equal").Return(Operator.Equal).Or(Parse.IgnoreCase("Like").Return(Operator.Like)).Token()
                from text in Grammar.QuotedTextual
                select new FilterCaseAction(variableName, @operator, text, negation.IsDefined)
        );

        public readonly static Parser<IAction> Parser =
        (
                from @case in Keyword.Case
                from action in CaseLoadFileParser.Or(CaseLoadQueryParser).Or(CaseRemoveParser).Or(CaseRenameParser).Or(CaseMoveParser).Or(CaseFilterParser)
                select action
        );
    }
}
