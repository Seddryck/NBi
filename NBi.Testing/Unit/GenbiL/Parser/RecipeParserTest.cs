using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class RecipeParserTest
    {
        [Test]
        public void SentenceParser_CaseLoadFileString_ValidCaseLoadSentence()
        {
            var input = "case load file 'filename.csv';";
            input += "case remove column 'first vraiable';";
            input += "template load file 'my template.nbitt';";
            input += "setting reference 'no way' connectionString 'youyou';;";
            input += "setting default assert connectionString 'youyou';";
            input += "suite generate;";
            input += "suite save 'filename.nbits';";

            var result = Recipe.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<LoadCaseAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<RemoveCaseAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<LoadTemplateAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<ReferenceAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<DefaultAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<GenerateSuiteAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<SaveSuiteAction>()));
        }
    }
}
