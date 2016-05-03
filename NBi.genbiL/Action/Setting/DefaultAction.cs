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
                case DefaultType.Everywhere: name = "Default - Everywhere";
                    break;
                case DefaultType.SystemUnderTest: name = "Default - System-under-test";
                    break;
                case DefaultType.Assert: name= "Default - Assert";
                    break;
                case DefaultType.SetupCleanup: name = "Default - Setup-cleanup";
                    break;
                default:
                    break;
            }

            state.Settings.SetValue(name, Value);
        }

        public string Display
        {
            get
            {
                return string.Format("Create {0} default value for {1} and defining it to '{2}'"
                    , GetLiteralForDefaulType(DefaultType)
                    , Variable
                    , Value
                    );
            }
        }

        private string GetLiteralForDefaulType(Action.DefaultType defaultType)
        {
            switch (defaultType)
            {
                case DefaultType.Everywhere: return "Everywhere";
                case DefaultType.SystemUnderTest: return "System-Under-Test";
                case DefaultType.Assert: return "Assert";
                case DefaultType.SetupCleanup: return "Setup-cleanup";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
