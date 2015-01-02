using System;
using System.Linq;
using NBi.Xml.Settings;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting
{
    public class DefaultAction : ISettingAction
    {
        public DefaultType DefaultType { get; set; }
        public string Value { get; set; }

        public DefaultAction(DefaultType defaultType, string value)
        {
            DefaultType = defaultType;
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            state.Settings.Defaults[DefaultType] = Value;
        }

        public string Display
        {
            get
            {
                return string.Format("Create {0} default value for connectionString and defining it to '{1}'"
                    , DefaultType==Action.DefaultType.SystemUnderTest ? "System-Under-Test" : "Assert"
                    , Value
                    );
            }
        }
    }
}
