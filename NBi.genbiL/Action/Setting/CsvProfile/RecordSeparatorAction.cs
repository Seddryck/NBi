using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class RecordSeparatorAction : ICsvProfileAction
{
    public string Value { get; set; }

    public RecordSeparatorAction(string value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new NBi.Model.Settings.CsvProfile());
        state.Settings.CsvProfile.RecordSeparator = Value;
    }

    public string Display => $"Create CSV profile setting named 'RecordSeparator' and defining it to '{Value}'";
}
