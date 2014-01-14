using System;
using System.Linq;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class SuiteParserTest
    {
        [Test]
        public void SentenceParser_SuiteGenerate_ValidGenerateSuiteAction()
        {
            var input = "suite generate;";
            var result = Suite.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GenerateSuiteAction>());
            Assert.That(((GenerateSuiteAction)result).Grouping, Is.False);
        }

        [Test]
        public void SentenceParser_SuiteGenerateGrouping_ValidGenerateSuiteAction()
        {
            var input = "suite generate grouping;";
            var result = Suite.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<GenerateSuiteAction>());
            Assert.That(((GenerateSuiteAction)result).Grouping, Is.True);
        }

        [Test]
        public void SentenceParser_SuiteSaveString_ValidSaveSuiteAction()
        {
            var input = "suite save 'filename.nbits';";
            var result = Suite.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<SaveSuiteAction>());
            Assert.That(((SaveSuiteAction)result).Filename, Is.EqualTo("filename.nbits"));
        }
    }
}
