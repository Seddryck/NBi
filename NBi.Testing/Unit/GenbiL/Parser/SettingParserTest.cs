using System;
using System.Linq;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class SettingParserTest
    {
        [Test]
        public void SentenceParser_TemplateLoadFileString_ValidTemplateLoadSentence()
        {
            var input = "setting default assert connectionString 'youyou';";
            var result = Setting.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<DefaultAction>());
            Assert.That(((DefaultAction)result).Value, Is.EqualTo("youyou"));
        }

        [Test]
        public void SentenceParser_TemplateLoadPredefinedString_ValidTemplateLoadSentence()
        {
            var input = "setting reference 'no way' connectionString 'youyou';";
            var result = Setting.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReferenceAction>());
            Assert.That(((ReferenceAction)result).Name, Is.EqualTo("no way"));
            Assert.That(((ReferenceAction)result).Value, Is.EqualTo("youyou"));
        }
    }
}
