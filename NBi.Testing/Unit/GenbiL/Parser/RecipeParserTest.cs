using System;
using System.Linq;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;
using NBi.GenbiL.Action;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class RecipeParserTest
    {
        [Test]
        public void Parser_LargeRecipe_ValidParsing()
        {
            var input = "";
            
            input += "case load file 'filename.csv';" + Environment.NewLine;
            input += "case remove column 'first vraiable';" + Environment.NewLine;
            input += "template load file 'my template.nbitt';" + Environment.NewLine;
            input += "setting reference 'no way' connectionString 'youyou';;" + Environment.NewLine;
            input += "setting default assert connectionString 'youyou';" + Environment.NewLine;
            input += "//Suite Generated" + Environment.NewLine;
            input += "suite generate;" + Environment.NewLine;
            input += "suite save 'filename.nbits';" + Environment.NewLine;

            var result = Recipe.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<LoadCaseFromFileAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<RemoveCaseAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<LoadExternalTemplateAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<ReferenceAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<DefaultAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<GenerateSuiteAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<EmptyAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<SaveSuiteAction>()));
        }

        public void SentenceParser_LargeRecipeWithMultilineActionsAndComments_ValidCaseLoadSentence()
        {
            var input = "";

            input += "case load file " + Environment.NewLine;
            input += "'filename.csv';" + Environment.NewLine;
            input += "/*Suite Generated" + Environment.NewLine;
            input += "New comment line*/" + Environment.NewLine;
            input += "suite generate;" + Environment.NewLine;

            var result = Recipe.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<LoadCaseFromFileAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<GenerateSuiteAction>()));
            Assert.That(result, Has.Some.Matches(Is.InstanceOf<EmptyAction>()));
        }
    }
}
