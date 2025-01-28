using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting;

public class ParallelizeQueriesAction : ISettingAction
{
    public bool Value { get; set; }

    public ParallelizeQueriesAction(bool value)
    {
        Value = value;
    }

    public void Execute(GenerationState state)
    {
        state.Settings.ParallelizeQueries = Value;
    }

    public string Display
    {
        get
        {
            return string.Format("Set parameter 'parallellize queries' to '{0}'."
                , Value ? "ON" : "OFF"
                );
        }
    }
}
