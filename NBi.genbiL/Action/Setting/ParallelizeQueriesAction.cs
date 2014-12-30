using System;
using System.Linq;
using NBi.Xml.Settings;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting
{
    public class ParallelizeQueriesAction : ISettingAction
    {
        public bool Value { get; set; }

        public ParallelizeQueriesAction(bool value)
        {
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            state.Settings.SetParallelizeQueries(Value);
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
}
