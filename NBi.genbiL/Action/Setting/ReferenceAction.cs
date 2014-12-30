using System;
using System.Linq;
using NBi.Xml.Settings;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting
{
    public class ReferenceAction : ISettingAction
    {
        public string Name { get; set; }
        public string Variable { get; set; }
        public string Value { get; set; }

        public ReferenceAction(string name, string variable, string value)
        {
            Name = name;
            Variable= variable;
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            if (Variable.ToLower() != "ConnectionString".ToLower())
                throw new ArgumentException("Currently you must define the variable as ConnectionString. Other options are not supported!");
            
            if (state.Settings.Exists(Name))
                state.Settings.SetValue(Name, Value);
            else
                state.Settings.Add(Name, Value);
        }

        public string Display
        {
            get
            {
                return string.Format("Create reference named '{0}' with value for {1} and defining it to '{2}'"
                    , Name
                    , Variable
                    , Value
                    );
            }
        }
    }
}
