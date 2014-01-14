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

            DefaultXml @default = null;
            var name = string.Empty;
            switch (DefaultType)
            {
                case DefaultType.SystemUnderTest: @default = state.Settings.DefaultSut; name= "Default - System-under-test";
                    break;
                case DefaultType.Assert: @default = state.Settings.DefaultAssert; name= "Default - Assert";
                    break;
                default:
                    break;
            }
            if (@default == null)
                state.Settings.Add(name, Value);
            else
                @default.ConnectionString = Value;
        }
    }
}
