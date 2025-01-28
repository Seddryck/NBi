using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class TextQualifierAction : ICsvProfileAction
{
    public char Value { get; set; }

    public TextQualifierAction(char value)
        => Value  = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
        state.Settings.CsvProfile.TextQualifier = Value;
    }

    public string Display => $"Create CSV profile setting named 'TextQualifier' and defining it to '{Value}'";
}
