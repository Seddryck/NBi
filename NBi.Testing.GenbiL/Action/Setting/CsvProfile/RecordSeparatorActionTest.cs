using NBi.GenbiL.Action.Setting.CsvProfile;
using NBi.GenbiL.Stateful;
using NUnit.Framework;

namespace NBi.GenbiL.Testing.Action.Setting.CsvProfile;

public class RecordSeparatorActionTest
{
    [Test]
    public void Execute_NewRecordSeparator_ProfileAdded()
    {
        var state = new GenerationState();

        var action = new RecordSeparatorAction("\r\n");

        action.Execute(state);
        var target = state.Settings.CsvProfile;
        Assert.That(target, Is.Not.Null);
    }

    
    [Test]
    public void Execute_NewCsvProfile_RecordSeparatorAdded()
    {
        var state = new GenerationState();

        var action = new RecordSeparatorAction("\r\n");
        action.Execute(state);
        var target = state.Settings.CsvProfile.RecordSeparator;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("\r\n"));
    }
    
    [Test]
    public void Execute_OverrideExistingNewCsvProfile_RecordSeparatorOverriden()
    {
        var state = new GenerationState();
        state.Settings.CsvProfile.RecordSeparator = "\r\n";

        var action = new RecordSeparatorAction("\n");
        action.Execute(state);
        var target = state.Settings.CsvProfile.RecordSeparator;
        Assert.That(target, Is.Not.Null);
        Assert.That(target, Is.EqualTo("\n"));
    }
}
