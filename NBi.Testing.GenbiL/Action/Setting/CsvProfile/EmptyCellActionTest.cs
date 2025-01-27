using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class EmptyCellActionTest
{
    [Test]
    public void Execute_NewEmptyCell_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new EmptyCellAction("NULL");

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_EmptyCellAdded()
    {
        var state = new GenerationState();

        var action = new EmptyCellAction("NULL");
        action.Execute(state);
        var target = state.Settings.CsvProfile.EmptyCell;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("NULL"));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_EmptyCellOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.EmptyCell = "originalValue";

        var action = new EmptyCellAction("newValue");
        action.Execute(state);
        var target = state.Settings.CsvProfile.EmptyCell;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("newValue"));
    }
}
