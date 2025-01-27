using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class MissingCellActionTest
{
    [Test]
    public void Execute_NewMissingCell_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new MissingCellAction("NULL");

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_MissingCellAdded()
    {
        var state = new GenerationState();

        var action = new MissingCellAction("NULL");
        action.Execute(state);
        var target = state.Settings.CsvProfile.MissingCell;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("NULL"));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_MissingCellOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.MissingCell = "originalValue";

        var action = new MissingCellAction("newValue");
        action.Execute(state);
        var target = state.Settings.CsvProfile.MissingCell;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("newValue"));
    }
}
