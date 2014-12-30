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
            Assert.That(result, Is.AssignableTo<ITemplateAction>());
            Assert.That(result, Is.InstanceOf<LoadExternalTemplateAction>());
            Assert.That(((LoadExternalTemplateAction)result).Path, Is.EqualTo("filename.nbitt"));
        }

        [Test]
        public void SentenceParser_TemplateLoadPredefinedString_ValidTemplateLoadSentence()
        {
            var input = "Template Load predefined 'SubsetOfHierarchy';";
            var result = Template.Parser.Parse(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.AssignableTo<ITemplateAction>());
            Assert.That(result, Is.InstanceOf<LoadPredefinedTemplateAction>());
            Assert.That(((LoadPredefinedTemplateAction)result).ResourceName, Is.EqualTo("SubsetOfHierarchy"));
        }
    }
}
