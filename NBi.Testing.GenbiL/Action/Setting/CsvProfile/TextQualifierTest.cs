using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class TextQualifierActionTest
{
    [Test]
    public void Execute_NewCsvProfileTextQualifier_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new TextQualifierAction('"');

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_TextQualifierAdded()
    {
        var state = new GenerationState();

        var action = new TextQualifierAction('"');
        action.Execute(state);
        var target = state.Settings.CsvProfile.TextQualifier;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo('"'));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_TextQualifierOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.TextQualifier = '"';

        var action = new TextQualifierAction('#');
        action.Execute(state);
        var target = state.Settings.CsvProfile.TextQualifier;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo('#'));
    }
}
