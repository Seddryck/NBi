using System;
using System.Linq;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting
{
    public class DefaultAction : ISettingAction
    {
        public DefaultType DefaultType { get; set; }
        public string Variable { get; set; }
        public string Value { get; set; }

        public DefaultAction(DefaultType defaultType, string variable, string value)
        {
            DefaultType = defaultType;
            Variable= variable;
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            if (Variable.ToLower() != "ConnectionString".ToLower())
                throw new ArgumentException("Currently you must define the variable as ConnectionString. Other options are not supported!");

            var name = string.Empty;
            switch (DefaultType)
            {
                case DefaultType.SystemUnderTest: name= "Default - System-Under-Test";
                    break;
                case DefaultType.Assert: name= "Default - Assert";
                    break;
                default:
                    break;
            }

            var @default = state.Settings.GetSettings().First(set => set.Name == name);
            @default.Value = Value;
        }
    }
}
