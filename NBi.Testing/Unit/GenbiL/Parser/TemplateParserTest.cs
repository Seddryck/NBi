using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.Testing.Unit.GenbiL.Parser
{
    [TestFixture]
    public class TemplateParserTest
    {
        [Test]
        public void SentenceParser_TemplateLoadFileString_ValidTemplateLoadSentence()
        {
            var input = "Template Load File 'filename.nbitt';";
            var result = Template.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadTemplateAction>());
            Assert.That(((LoadTemplateAction)result).LoadType, Is.EqualTo(LoadType.File));
            Assert.That(((LoadTemplateAction)result).Filename, Is.EqualTo("filename.nbitt"));
        }

        [Test]
        public void SentenceParser_TemplateLoadPredefinedString_ValidTemplateLoadSentence()
        {
            var input = "Template Load predefined 'SubsetOfHierarchy';";
            var result = Template.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<LoadTemplateAction>());
            Assert.That(((LoadTemplateAction)result).LoadType, Is.EqualTo(LoadType.Predefined));
            Assert.That(((LoadTemplateAction)result).Filename, Is.EqualTo("SubsetOfHierarchy"));
        }
    }
}
