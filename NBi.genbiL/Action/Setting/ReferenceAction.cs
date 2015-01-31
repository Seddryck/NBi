using System;
using System.Linq;
using NBi.Xml.Settings;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Setting
{
    public class ReferenceAction : ISettingAction
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ReferenceAction(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            if (state.Settings.References.Keys.Contains(Name))
                state.Settings.References[Name] = Value;
            else
                state.Settings.References.Add(Name, Value);
        }

        public string Display
        {
            get
            {
                return string.Format("Create reference named '{0}' with value with connectionString defined as '{1}'"
                    , Name
                    , Value
                    );
            }
        }
    }
}
