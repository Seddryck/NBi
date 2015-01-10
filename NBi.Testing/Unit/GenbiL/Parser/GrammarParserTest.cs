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
    public class GrammarParserTest
    {
        [Test]
        public void ExtendedQuotedTextual_QuotedText_ReturnValue()
        {
            var input = "'alpha'";
            var result = Grammar.ExtendedQuotedTextual.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("alpha"));
        }

        [Test]
        public void ExtendedQuotedTextual_Empty_ReturnValue()
        {
            var input = "empty";
            var result = Grammar.ExtendedQuotedTextual.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void ExtendedQuotedTextual_None_ReturnValue()
        {
            var input = "none";
            var result = Grammar.ExtendedQuotedTextual.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("(none)"));
        }

        [Test]
        public void ExtendedQuotedRecordSequence_ValueNoneValueEmpty_ReturnFourResults()
        {
            var input = "'alpha', none,'beta',empty";
            var result = Grammar.ExtendedQuotedRecordSequence.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Member("(none)"));
            Assert.That(result, Has.Member(""));
            Assert.That(result, Has.Member("alpha"));
            Assert.That(result, Has.Member("beta"));
            Assert.That(result.Count(), Is.EqualTo(4));
        }

    }
}
