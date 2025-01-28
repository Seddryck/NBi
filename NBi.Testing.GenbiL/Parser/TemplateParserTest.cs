using System;
using System.Linq;
using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Parser;
using NUnit.Framework;
using Sprache;

namespace NBi.GenbiL.Testing.Parser;

[TestFixture]
public class TemplateParserTest
{
    [Test]
    public void SentenceParser_TemplateLoadFileString_ValidTemplateLoadSentence()
    {
        var input = "Template Load File 'filename.nbitt';";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<LoadFileTemplateAction>());
    }

    [Test]
    public void SentenceParser_TemplateAddFileString_ValidTemplateLoadSentence()
    {
        var input = "Template Add File 'filename.nbitt';";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<AddFileTemplateAction>());
        Assert.That(((AddFileTemplateAction)result).Filename, Is.EqualTo("filename.nbitt"));
    }

    [Test]
    public void SentenceParser_TemplateLoadEmbeddedString_ValidTemplateLoadSentence()
    {
        var input = "Template Load predefined 'SubsetOfHierarchy';";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<LoadEmbeddedTemplateAction>());
    }

    [Test]
    public void SentenceParser_TemplateAddEmbeddedWithPredefinedString_ValidTemplateLoadSentence()
    {
        var input = "Template Add predefined 'ExistsDimensions';";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<AddEmbeddedTemplateAction>());
        Assert.That(((AddEmbeddedTemplateAction)result).Filename, Is.EqualTo("ExistsDimensions"));
    }

    [Test]
    public void SentenceParser_TemplateAddEmbeddedString_ValidTemplateLoadSentence()
    {
        var input = "Template Add embedded 'ExistsDimensions';";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<AddEmbeddedTemplateAction>());
        Assert.That(((AddEmbeddedTemplateAction)result).Filename, Is.EqualTo("ExistsDimensions"));
    }

    [Test]
    public void SentenceParser_TemplateClearString_ValidTemplateClearSentence()
    {
        var input = "template clear;";
        var result = Template.Parser.Parse(input);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ClearTemplateAction>());
    }
}
