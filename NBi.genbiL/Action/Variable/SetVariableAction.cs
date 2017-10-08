using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Variable
{
    public class SetVariableAction : IVariableAction
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public SetVariableAction(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public void Execute(GenerationState state)
        {
            if (state.Variables.ContainsKey(Name))
                state.Variables[Name] = Value;
            else
                state.Variables.Add(Name, Value);
        }

        public string Display
        {
            get
            {
                return string.Format($"Setting value '{Value}' for variable '${Name}$'");
            }
        }
    }
}
