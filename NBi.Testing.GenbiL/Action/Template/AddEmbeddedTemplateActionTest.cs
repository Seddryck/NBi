using NBi.GenbiL;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Template;

public class AddEmbeddedTemplateActionTest
{
    [Test]
    public void Display_AddEmbeddedTemplateAction_CorrectString()
    {
        
        var action = new AddEmbeddedTemplateAction("ExistsDimension");
        Assert.That(action.Display, Is.EqualTo("Adding new template from embedded resource 'ExistsDimension'"));
    }

    [Test]
    [TestCase("ExistsDimension")]
    [TestCase("ContainedInDimensions")]
    [TestCase("SubsetOfDimensions")]
    public void Execute_AddEmbeddedTemplateAction_TemplateLoaded(string name)
    {
        var state = new GenerationState();
        var action = new AddEmbeddedTemplateAction(name);
        action.Execute(state);

        Assert.That(state.Templates, Has.Count.EqualTo(1));
        Assert.That(state.Templates.ElementAt(0), Is.Not.Empty);
    }
}
