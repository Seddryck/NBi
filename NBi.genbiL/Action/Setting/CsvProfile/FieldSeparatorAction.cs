using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class FieldSeparatorAction : ICsvProfileAction
{
    public char Value { get; set; }

    public FieldSeparatorAction(char value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new NBi.Model.Settings.CsvProfile());
        state.Settings.CsvProfile.FieldSeparator = Value;
    }

    public string Display => $"Create CSV profile setting named 'FieldSeparator' and defining it to '{Value}'";
}
