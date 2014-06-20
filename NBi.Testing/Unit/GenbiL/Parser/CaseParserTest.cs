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
            Assert.That(result, Is.InstanceOf<LoadCaseFromFileAction>());
            Assert.That(((LoadCaseFromFileAction)result).Filename, Is.EqualTo("filename.csv"));
        }

        [Test]
        public void SentenceParser_CaseLoadQueryFileString_ValidCaseLoadSentence()
        {
            var input = "case load query 'filename.sql' on 'connStr';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadCaseFromQueryFileAction>());
            Assert.That(((LoadCaseFromQueryFileAction)result).Filename, Is.EqualTo("filename.sql"));
            Assert.That(((LoadCaseFromQueryFileAction)result).ConnectionString, Is.EqualTo("connStr"));
        }

        [Test]
        public void SentenceParser_CaseLoadQueryString_ValidCaseLoadSentence()
        {
            var input = "case load query \r\n {select distinct myField from myTable} \r\non 'connStr';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadCaseFromQueryAction>());
            Assert.That(((LoadCaseFromQueryAction)result).Query, Is.EqualTo("select distinct myField from myTable"));
            Assert.That(((LoadCaseFromQueryAction)result).ConnectionString, Is.EqualTo("connStr"));
        }

        [Test]
        public void SentenceParser_CaseRemoveColumnString_ValidCaseRemoveColumn()
        {
            var input = "case remove column 'perspective'";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RemoveCaseAction>());
            Assert.That(((RemoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
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
        public void SentenceParser_CaseMoveColumnString_ValidCaseMoveColumn()
        {
            var input = "case move column 'perspective' to left";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<MoveCaseAction>());
            Assert.That(((MoveCaseAction)result).VariableName, Is.EqualTo("perspective"));
            Assert.That(((MoveCaseAction)result).RelativePosition, Is.EqualTo(-1));
        }

        [Test]
        public void SentenceParser_CaseFilterEqual_ValidFilterAction()
        {
            var input = "case filter on column 'perspective' values equal 'hidden-perspective';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterCaseAction>());
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("hidden-perspective"));
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
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("show-perspective"));
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
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("start%"));
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
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("%end"));
            Assert.That(((FilterCaseAction)result).Negation, Is.EqualTo(true));
            Assert.That(((FilterCaseAction)result).Operator, Is.EqualTo(Operator.Like));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }
    }
}
