using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class FirstRowHeaderActionTest
{
    [Test]
    public void Execute_NewFirstRowHeader_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new FirstRowHeaderAction(true);

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_FirstRowHeaderAdded()
    {
        var state = new GenerationState();

        var action = new FirstRowHeaderAction(true);
        action.Execute(state);
        var target = state.Settings.CsvProfile.FirstRowHeader;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo(true));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_FirstRowHeaderOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.FirstRowHeader = true;

        var action = new FirstRowHeaderAction(false);
        action.Execute(state);
        var target = state.Settings.CsvProfile.FirstRowHeader;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo(false));
    }
}
