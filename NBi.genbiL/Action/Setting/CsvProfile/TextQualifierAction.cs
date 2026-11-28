using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class TextQualifierAction : ICsvProfileAction
{
    public char Value { get; set; }

    public TextQualifierAction(char value)
        => Value  = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new NBi.Model.Settings.CsvProfile());
        state.Settings.CsvProfile.TextQualifier = Value;
    }

    public string Display => $"Create CSV profile setting named 'TextQualifier' and defining it to '{Value}'";
}
