using System;
using System.Linq;
using NBi.Xml.Settings;

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
            
            var name = string.Format("Reference - {0}", Name);
            if (state.Settings.Exists(name))
                state.Settings.SetValue(name, Value);
            else
                state.Settings.Add(name, Value);
        }
    }
}
