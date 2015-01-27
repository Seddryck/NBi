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
                select new LoadFromFileCaseAction(filename)
        );

        readonly static Parser<ICaseAction> caseLoadQueryFileParser =
        (
                from load in Keyword.Load
                from loadType in loadTypeQueryParser
                from filename in Grammar.QuotedTextual
                from onKeyword in Keyword.On
                from connectionString in Grammar.QuotedTextual
                select new LoadFromQueryFileCaseAction(filename, connectionString)
        );

        readonly static Parser<ICaseAction> caseLoadQueryParser =
        (
                from load in Keyword.Load
                from loadType in loadTypeQueryParser
                from query in Grammar.CurlyBraceTextual
                from onKeyword in Keyword.On
                from connectionString in Grammar.QuotedTextual
                select new LoadFromQueryCaseAction(query, connectionString)
        );

        readonly static Parser<ICaseAction> caseLoadParser =
            caseLoadFileParser.Or(caseLoadQueryFileParser).Or(caseLoadQueryParser);

        readonly static Parser<ICaseAction> caseRemoveParser =
        (
                from remove in Keyword.Remove
                from axisType in axisTypeParser
                from variableNames in Grammar.QuotedRecordSequence
                select new RemoveCaseAction(variableNames)
        );

        readonly static Parser<ICaseAction> caseHoldParser =
        (
                from remove in Keyword.Hold
                from axisType in axisTypeParser
                from variables in Grammar.QuotedRecordSequence
                select new HoldCaseAction(variables)
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
                from relativePosition in Parse.IgnoreCase("Left").Return(-1)
                                                .Or(Parse.IgnoreCase("Right").Return(1))
                                                .Or(Parse.IgnoreCase("First").Return(int.MinValue))
                                                .Or(Parse.IgnoreCase("Last").Return(int.MaxValue))
                                            .Token()
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
                from text in Grammar.ExtendedQuotedRecordSequence
                select new FilterCaseAction(variableName, @operator, text, negation.IsDefined)
        );

        readonly static Parser<ICaseAction> caseScopeParser =
        (
                from scope in Keyword.Scope
                from name in Grammar.QuotedTextual
                select new ScopeCaseAction(name)
        );

        readonly static Parser<ICaseAction> caseCrossFullParser =
        (
                from cross in Keyword.Cross
                from first in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from second in Grammar.QuotedTextual
                select new CrossFullCaseAction(first, second)
        );

        readonly static Parser<ICaseAction> caseCrossOnColumnParser =
        (
                from cross in Keyword.Cross
                from first in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from second in Grammar.QuotedTextual
                from onKeyword in Keyword.On
                from matchingColumn in Grammar.QuotedTextual
                select new CrossColumnMatchingCaseAction(first, second, matchingColumn)
        );

        readonly static Parser<ICaseAction> caseCrossVectorParser =
        (
                from cross in Keyword.Cross
                from first in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from vectorKeyword in Keyword.Vector
                from vectorName in Grammar.QuotedTextual
                from valuesKeyword in Keyword.Values
                from values in Grammar.QuotedRecordSequence
                select new CrossVectorCaseAction(first, vectorName, values)
        );

        readonly static Parser<ICaseAction> caseSaveParser =
        (
                from save in Keyword.Save
                from @as in Keyword.As
                from filename in Grammar.QuotedTextual
                select new SaveCaseAction(filename)
        );

        readonly static Parser<ICaseAction> caseCopyParser =
        (
                from copy in Keyword.Copy
                from @from in Grammar.QuotedTextual
                from toKeyword in Keyword.To
                from @to in Grammar.QuotedTextual
                select new CopyCaseAction(@from, @to)
        );

        readonly static Parser<ICaseAction> caseFilterDistinctParser =
        (
                from filter in Keyword.Filter
                from onKeyword in Keyword.Distinct
                select new FilterDistinctCaseAction()
        );

        readonly static Parser<ICaseAction> caseAddParser =
        (
                from add in Keyword.Add
                from axisType in axisTypeParser
                from columnName in Grammar.QuotedTextual
                select new AddCaseAction(columnName)
        );

        readonly static Parser<ICaseAction> caseAddWithDefaultParser =
        (
                from add in Keyword.Add
                from axisType in axisTypeParser
                from columnName in Grammar.QuotedTextual
                from valuesKeyword in Keyword.Values
                from defaultValue in Grammar.QuotedTextual
                select new AddCaseAction(columnName, defaultValue)
        );

        readonly static Parser<ICaseAction> caseMergeParser =
        (
                from add in Keyword.Merge
                from withKeyword in Keyword.With
                from scopeName in Grammar.QuotedTextual
                select new MergeCaseAction(scopeName)
        );

        readonly static Parser<ICaseAction> caseConcatenateParser =
        (
                from concatenate in Keyword.Concatenate
                from axisType in Parse.IgnoreCase("Column").Token()
                from columnName in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from valuables in Grammar.Valuables
                select new ConcatenateCaseAction(columnName, valuables)
        );

        readonly static Parser<ICaseAction> caseSubstituteParser =
        (
                from substitute in Keyword.Substitute
                from intoKeyword in Keyword.Into
                from axisType in Parse.IgnoreCase("Column").Token()
                from columnName in Grammar.QuotedTextual
                from oldText in Grammar.Valuable
                from withKeyword in Keyword.With
                from newText in Grammar.Valuable
                select new SubstituteCaseAction(columnName, oldText, newText)
        );

        readonly static Parser<ICaseAction> caseReplaceSimpleParser =
        (
                from replace in Keyword.Replace
                from axisType in Parse.IgnoreCase("Column").Token()
                from variableName in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from valuesKeyword in Keyword.Values
                from text in Grammar.ExtendedQuotedTextual
                select new ReplaceCaseAction(variableName, text)
        );

        readonly static Parser<ICaseAction> caseReplaceConditionalParser =
        (
                from replace in Keyword.Replace
                from axisType in Parse.IgnoreCase("Column").Token()
                from variableName in Grammar.QuotedTextual
                from withKeyword in Keyword.With
                from valuesKeyword in Keyword.Values
                from newValue in Grammar.ExtendedQuotedTextual
                from whenKeyword in Keyword.When
                from valuesKeyword2 in Keyword.Values
                from negation in Keyword.Not.Optional()
                from @operator in Parse.IgnoreCase("Equal").Return(Operator.Equal).Or(Parse.IgnoreCase("Like").Return(Operator.Like)).Token()
                from text in Grammar.ExtendedQuotedRecordSequence
                select new ReplaceConditionalCaseAction(variableName, newValue, @operator, text, negation.IsDefined)
        );

        public readonly static Parser<IAction> Parser =
        (
                from @case in Keyword.Case
                from action in caseLoadParser
                                    .Or(caseRemoveParser)
                                    .Or(caseHoldParser)
                                    .Or(caseRenameParser)
                                    .Or(caseMoveParser)
                                    .Or(caseFilterParser)
                                    .Or(caseFilterDistinctParser)
                                    .Or(caseScopeParser)
                                    .Or(caseCrossOnColumnParser)
                                    .Or(caseCrossFullParser)
                                    .Or(caseCrossVectorParser)
                                    .Or(caseSaveParser)
                                    .Or(caseCopyParser)
                                    .Or(caseAddWithDefaultParser)
                                    .Or(caseAddParser)
                                    .Or(caseMergeParser)
                                    .Or(caseReplaceConditionalParser)
                                    .Or(caseReplaceSimpleParser)
                                    .Or(caseConcatenateParser)
                                    .Or(caseSubstituteParser)
                select action
        );
    }
}
