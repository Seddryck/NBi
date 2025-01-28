using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class RecordSeparatorAction : ICsvProfileAction
{
    public string Value { get; set; }

    public RecordSeparatorAction(string value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
        state.Settings.CsvProfile.RecordSeparator = Value;
    }

    public string Display => $"Create CSV profile setting named 'RecordSeparator' and defining it to '{Value}'";
}
