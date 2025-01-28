using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class FieldSeparatorActionTest
{
    [Test]
    public void Execute_NewFieldSeparator_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new FieldSeparatorAction('|');

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_FieldSeparatorAdded()
    {
        var state = new GenerationState();

        var action = new FieldSeparatorAction('|');
        action.Execute(state);
        var target = state.Settings.CsvProfile.FieldSeparator;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo('|'));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_FieldSeparatorOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.FieldSeparator = '|';

        var action = new FieldSeparatorAction('\t');
        action.Execute(state);
        var target = state.Settings.CsvProfile.FieldSeparator;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo('\t'));
    }
}
