using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Case;
using NBi.Service;
using Sprache;

namespace NBi.GenbiL.Parser
{
    class Case
    {
        readonly static  Parser<LoadType> loadTypeFileParser =
            Parse.IgnoreCase("file").Return(LoadType.File)
                .Token();

        readonly static Parser<LoadType> loadTypeQueryParser =
            Parse.IgnoreCase("query").Return(LoadType.Query)
                .Token();

        readonly static Parser<AxisType> axisTypeParser =
            Parse.IgnoreCase("column").Return(AxisType.Column)
                .Token();

        readonly static Parser<ICaseAction> caseLoadFileParser =
        (
                from load in Keyword.Load
                from loadType in loadTypeFileParser
                from filename in Grammar.QuotedTextual
                select new LoadCaseFromFileAction(filename)
        );

        readonly static Parser<ICaseAction> caseLoadQueryFileParser =
        (
                from load in Keyword.Load
                from loadType in loadTypeQueryParser
                from filename in Grammar.QuotedTextual
                from onKeyword in Keyword.On
                from connectionString in Grammar.QuotedTextual
                select new LoadCaseFromQueryFileAction(filename, connectionString)
        );

        readonly static Parser<ICaseAction> caseLoadQueryParser =
        (
                from load in Keyword.Load
                from loadType in loadTypeQueryParser
                from query in Grammar.CurlyBraceTextual
                from onKeyword in Keyword.On
                from connectionString in Grammar.QuotedTextual
                select new LoadCaseFromQueryAction(query, connectionString)
        );

        readonly static Parser<ICaseAction> caseLoadParser =
            caseLoadFileParser.Or(caseLoadQueryFileParser).Or(caseLoadQueryParser);

        readonly static Parser<ICaseAction> caseRemoveParser =
        (
                from remove in Keyword.Remove
                from axisType in axisTypeParser
                from variableName in Grammar.QuotedTextual
                select new RemoveCaseAction(variableName)
        );

        readonly static Parser<ICaseAction> caseRenameParser =
        (
                from remove in Keyword.Rename
                from axisType in Parse.IgnoreCase("Column").Token()
                from oldVariableName in Grammar.QuotedTextual
                from intoKeyword in Keyword.Into
                from newVariableName in Grammar.QuotedTextual
                select new RenameCaseAction(oldVariableName, newVariableName)
        );

        readonly static Parser<ICaseAction> caseMoveParser =
        (
                from move in Keyword.Move
                from axisType in Parse.IgnoreCase("Column").Token()
                from variableName in Grammar.QuotedTextual
                from toKeyword in Keyword.To
                from relativePosition in Parse.IgnoreCase("Left").Return(-1).Or(Parse.IgnoreCase("Right").Return(1)).Token()
                select new MoveCaseAction(variableName, relativePosition)
        );

        readonly static Parser<ICaseAction> caseFilterParser =
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
                from action in caseLoadParser.Or(caseRemoveParser).Or(caseRenameParser).Or(caseMoveParser).Or(caseFilterParser)
                select action
        );
    }
}
