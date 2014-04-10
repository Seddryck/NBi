using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser;
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
        public void SentenceParser_CaseLoadQueryString_ValidCaseLoadSentence()
        {
            var input = "case load query 'filename.sql' on 'connStr';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadCaseFromQueryAction>());
            Assert.That(((LoadCaseFromQueryAction)result).Filename, Is.EqualTo("filename.sql"));
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
        public void SentenceParser_CaseFilterOut_ValidFilterOutAction()
        {
            var input = "case filter out 'hidden-perspective' on column 'perspective';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterOutCaseAction>());
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("hidden-perspective"));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }

        [Test]
        public void SentenceParser_CaseFilterIn_ValidFilterOutAction()
        {
            var input = "case filter in 'keep-perspective' on column 'perspective';";
            var result = Case.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<FilterInCaseAction>());
            Assert.That(((FilterCaseAction)result).Text, Is.EqualTo("keep-perspective"));
            Assert.That(((FilterCaseAction)result).Column, Is.EqualTo("perspective"));
        }
    }
}
