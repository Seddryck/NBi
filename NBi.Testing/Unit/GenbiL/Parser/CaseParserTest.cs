using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser;
using NBi.Service;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class CaseParserTest
    {
        [Test]
        public void SentenceParser_CaseLoadFileString_ValidCaseLoadSentence()
        {
            var input = "case load file 'filename.csv';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadFromFileCaseAction>());
            Assert.That(((LoadFromFileCaseAction)result).Filename, Is.EqualTo("filename.csv"));
        }

        [Test]
        public void SentenceParser_CaseLoadQueryFileString_ValidCaseLoadSentence()
        {
            var input = "case load query 'filename.sql' on 'connStr';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadFromQueryFileCaseAction>());
            Assert.That(((LoadFromQueryFileCaseAction)result).Filename, Is.EqualTo("filename.sql"));
            Assert.That(((LoadFromQueryFileCaseAction)result).ConnectionString, Is.EqualTo("connStr"));
        }

        [Test]
        public void SentenceParser_CaseLoadQueryString_ValidCaseLoadSentence()
        {
            var input = "case load query \r\n {select distinct myField from myTable} \r\non 'connStr';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadFromQueryCaseAction>());
            Assert.That(((LoadFromQueryCaseAction)result).Query, Is.EqualTo("select distinct myField from myTable"));
            Assert.That(((LoadFromQueryCaseAction)result).ConnectionString, Is.EqualTo("connStr"));
        }

        [Test]
        public void SentenceParser_CaseRemoveColumnString_ValidCaseRemoveColumn()
        {
            var input = "case remove column 'perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RemoveCaseAction>());
            Assert.That(((RemoveCaseAction)result).Variables[0], Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseRemoveColumnsString_ValidCaseRemoveColumn()
        {
            var input = "case remove column 'perspective', 'dimension'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RemoveCaseAction>());
            Assert.That(((RemoveCaseAction)result).Variables, Has.Member("perspective"));
            Assert.That(((RemoveCaseAction)result).Variables, Has.Member("dimension"));
        }

        [Test]
        public void SentenceParser_CaseHoldColumnString_ValidCaseHoldColumn()
        {
            var input = "case hold column 'perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<HoldCaseAction>());
            Assert.That(((HoldCaseAction)result).Variables[0], Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseHoldColumnsString_ValidCaseHoldColumn()
        {
            var input = "case hold column 'perspective', 'dimension'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<HoldCaseAction>());
            Assert.That(((HoldCaseAction)result).Variables, Has.Member("perspective"));
            Assert.That(((HoldCaseAction)result).Variables, Has.Member("dimension"));
        }

        [Test]
        public void SentenceParser_CaseRenameColumnString_ValidCaseRenameColumn()
        {
            var input = "case rename column 'perspective' into 'new perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RenameCaseAction>());
            Assert.That(((RenameCaseAction)result).OldVariableName, Is.EqualTo("perspective"));
            Assert.That(((RenameCaseAction)result).NewVariableName, Is.EqualTo("new perspective"));
        }

        [Test]
        public void SentenceParser_CaseMoveColumnStringLeft_ValidCaseMoveColumn()
        {
            var input = "case move column 'perspective' to left";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MoveCaseAction>());
            Assert.That(((MoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((MoveCaseAction)result).Position, Is.EqualTo(-1));
        }

        [Test]
        public void SentenceParser_CaseMoveColumnStringRight_ValidCaseMoveColumn()
        {
            var input = "case move column 'perspective' to right";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MoveCaseAction>());
            Assert.That(((MoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((MoveCaseAction)result).Position, Is.EqualTo(1));
        }

        [Test]
        public void SentenceParser_CaseMoveColumnStringFirst_ValidCaseMoveColumn()
        {
            var input = "case move column 'perspective' to first";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MoveCaseAction>());
            Assert.That(((MoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((MoveCaseAction)result).Position, Is.EqualTo(int.MinValue));
        }

        [Test]
        public void SentenceParser_CaseMoveColumnStringLast_ValidCaseMoveColumn()
        {
            var input = "case move column 'perspective' to last";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MoveCaseAction>());
            Assert.That(((MoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((MoveCaseAction)result).Position, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void SentenceParser_CaseFilterEqual_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values equal 'hidden-perspective';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member("hidden-perspective"));
            Assert.That(((FilterCaseAction)result).Values.Count(), Is.EqualTo(1));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(false));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterEmpty_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values equal empty;";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member(""));
            Assert.That(((FilterCaseAction)result).Values.Count(), Is.EqualTo(1));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(false));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterMixedQuotedAndNot_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values equal empty, 'alpha', none;";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member(""));
            Assert.That(((FilterCaseAction)result).Values, Has.Member("alpha"));
            Assert.That(((FilterCaseAction)result).Values, Has.Member("(none)"));
            Assert.That(((FilterCaseAction)result).Values.Count(), Is.EqualTo(3));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(false));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterNotEqual_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values not equal 'show-perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member("show-perspective"));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(true));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterLike_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values like 'start%'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member("start%"));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(false));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Like));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterNotLike_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values not like '%end'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member("%end"));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(true));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Like));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterMultiplesValues_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values not like '%end', 'start%'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Values, Has.Member("%end"));
            Assert.That(((FilterCaseAction)result).Values, Has.Member("start%"));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(true));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Like));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterDistinct_ValidFilterAction()
        {
            var input = "case filter distinct";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterDistinctCaseAction>());
        }

        public void SentenceParser_CaseFocus_ValidFocusAction()
        {
            var input = "case scope 'alpha'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ScopeCaseAction>());
        }

        public void SentenceParser_CaseCross_ValidCrossAction()
        {
            var input = "case cross 'alpha' with 'beta'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CrossColumnMatchingCaseAction>());

            var crossCase = result as CrossColumnMatchingCaseAction;
            Assert.That(crossCase.FirstSet, Is.EqualTo("alpha"));
            Assert.That(crossCase.SecondSet, Is.EqualTo("beta"));
            Assert.That(crossCase.MatchingColumn, Is.Null.Or.Empty);
        }

        public void SentenceParser_CaseCrossOnColumn_ValidCrossAction()
        {
            var input = "case cross 'alpha' with 'beta' on 'myKey'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CrossColumnMatchingCaseAction>());

            var crossCase = result as CrossColumnMatchingCaseAction;
            Assert.That(crossCase.FirstSet, Is.EqualTo("alpha"));
            Assert.That(crossCase.SecondSet, Is.EqualTo("beta"));
            Assert.That(crossCase.MatchingColumn, Is.EqualTo("myKey"));
        }

        public void SentenceParser_CaseCrossWithVector_ValidCrossAction()
        {
            var input = "case cross 'alpha' with vector 'beta' values 'value1', 'value2'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CrossVectorCaseAction>());

            var crossCase = result as CrossVectorCaseAction;
            Assert.That(crossCase.FirstSet, Is.EqualTo("alpha"));
            Assert.That(crossCase.VectorName, Is.EqualTo("beta"));
            Assert.That(crossCase.Values, Has.Member("value1"));
            Assert.That(crossCase.Values, Has.Member("value2"));
        }

        public void SentenceParser_CaseSave_ValidSaveAction()
        {
            var input = "case save as 'myfile.csv'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<SaveCaseAction>());

            var saveCase = result as SaveCaseAction;
            Assert.That(saveCase.Filename, Is.EqualTo("myfile.csv"));
        }

        public void SentenceParser_CaseCopy_ValidCopyAction()
        {
            var input = "case copy 'master' to 'copied-to'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CopyCaseAction>());

            var copyCase = result as CopyCaseAction;
            Assert.That(copyCase.From, Is.EqualTo("master"));
            Assert.That(copyCase.To, Is.EqualTo("copied-to"));
        }

        [Test]
        public void SentenceParser_CaseAddColumnStringWithoutDefault_ValidCaseAddColumn()
        {
            var input = "case add column 'perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<AddCaseAction>());
            Assert.That(((AddCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((AddCaseAction)result).DefaultValue, Is.EqualTo("(none)"));
        }

        [Test]
        public void SentenceParser_CaseAddColumnStringWithDefault_ValidCaseAddColumn()
        {
            var input = "case add column 'perspective' values '2014-07-01'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<AddCaseAction>());
            Assert.That(((AddCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((AddCaseAction)result).DefaultValue, Is.EqualTo("2014-07-01"));
        }

        [Test]
        public void SentenceParser_CaseMerge_ValidMergeAction()
        {
            var input = "case merge with 'scoped-value'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MergeCaseAction>());
            Assert.That(((MergeCaseAction)result).MergedScope, Is.EqualTo("scoped-value"));
        }

        [Test]
        public void SentenceParser_CaseReplaceNoCondition_ValidReplaceAction()
        {
            var input = "case replace column 'alpha' with values 'my new value'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReplaceCaseAction>());
            Assert.That(((ReplaceCaseAction)result).Column, Is.EqualTo("alpha"));
            Assert.That(((ReplaceCaseAction)result).NewValue, Is.EqualTo("my new value"));
        }

        [Test]
        public void SentenceParser_CaseReplaceWithcondition_ValidReplaceAction()
        {
            var input = "case replace column 'alpha' with values 'my new value' when values not equal 'foo', empty, 'bar';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReplaceConditionalCaseAction>());
            Assert.That(((ReplaceConditionalCaseAction)result).Column, Is.EqualTo("alpha"));
            Assert.That(((ReplaceConditionalCaseAction)result).NewValue, Is.EqualTo("my new value"));
            Assert.That(((ReplaceConditionalCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((ReplaceConditionalCaseAction)result).Negation, Is.True);
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member("foo"));
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member("bar"));
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member(""));
        }

        [Test]
        public void SentenceParser_CaseReplaceSpecialWithcondition_ValidReplaceAction()
        {
            var input = "case replace column 'alpha' with values none when values not equal 'foo', empty, 'bar';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReplaceConditionalCaseAction>());
            Assert.That(((ReplaceConditionalCaseAction)result).Column, Is.EqualTo("alpha"));
            Assert.That(((ReplaceConditionalCaseAction)result).NewValue, Is.EqualTo("(none)"));
            Assert.That(((ReplaceConditionalCaseAction)result).Operator, Is.EqualTo(Operator.Equal));
            Assert.That(((ReplaceConditionalCaseAction)result).Negation, Is.True);
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member("foo"));
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member("bar"));
            Assert.That(((ReplaceConditionalCaseAction)result).Values, Has.Member(""));
        }

        [Test]
        public void SentenceParser_CaseConcatenateColumns_ValidConcatenateAction()
        {
            var input = "case concatenate column 'alpha' with columns 'foo', 'bar';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ConcatenateCaseAction>());
            Assert.That(((ConcatenateCaseAction)result).ColumnName, Is.EqualTo("alpha"));
            Assert.That(((ConcatenateCaseAction)result).Valuables.Select(x => x.Display), Is.EquivalentTo(new[] {"column 'foo'", "column 'bar'"}));
        }

        [Test]
        public void SentenceParser_CaseConcatenateValue_ValidConcatenateAction()
        {
            var input = "case concatenate column 'alpha' with value 'foo';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ConcatenateCaseAction>());
            Assert.That(((ConcatenateCaseAction)result).ColumnName, Is.EqualTo("alpha"));
            Assert.That(((ConcatenateCaseAction)result).Valuables.Select(x => x.Display), Is.EquivalentTo(new[] { "value 'foo'" }));
        }

        [Test]
        public void SentenceParser_CaseSubstituteValue_ValidSubstituteAction()
        {
            var input = "case substitute into column 'beta' column 'alpha' with value 'foo';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<SubstituteCaseAction>());
            Assert.That(((SubstituteCaseAction)result).ColumnName, Is.EqualTo("beta"));
            Assert.That(((SubstituteCaseAction)result).OldText.Display, Is.EqualTo("column 'alpha'"));
            Assert.That(((SubstituteCaseAction)result).NewText.Display, Is.EqualTo("value 'foo'"));
        }

    }
}
