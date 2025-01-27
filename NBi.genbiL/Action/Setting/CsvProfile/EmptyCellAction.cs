using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting.CsvProfile;

public class EmptyCellAction : ICsvProfileAction
{
    public string Value { get; set; }

    public EmptyCellAction(string value)
        => Value = value;

    public void Execute(GenerationState state)
    {
        state.Settings.CsvProfile = (state.Settings.CsvProfile ?? new CsvProfileXml());
        state.Settings.CsvProfile.EmptyCell = Value;
    }

    public string Display => $"Create CSV profile setting named 'EmptyCell' and defining it to '{Value}'";
}
