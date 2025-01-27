using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class MissingCellAction : ICsvProfileAction
{
    public string Value { get; set; }

    public MissingCellAction(string value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
        state.Settings.CsvProfile.MissingCell = Value;
    }

    public string Display => $"Create CSV profile setting named 'MissingCell' and defining it to '{Value}'";
}
