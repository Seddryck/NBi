using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class FirstRowHeaderAction : ICsvProfileAction
{
    public bool Value { get; set; }

    public FirstRowHeaderAction(bool value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
        state.Settings.CsvProfile.FirstRowHeader = Value;
    }

    public string Display => $"Create CSV profile setting named 'FirstRowHeader' and defining it to '{Value}'";
}
