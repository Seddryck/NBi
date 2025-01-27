using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Consumable;

public class SetConsumableAction : IConsumableAction
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    public SetConsumableAction(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public void Execute(GenerationState state)
    {
        if (state.Consumables.ContainsKey(Name))
            state.Consumables[Name] = Value;
        else
            state.Consumables.Add(Name, Value);
    }

    public string Display
    {
        get
        {
            return string.Format($"Setting value '{Value}' for consumable '${Name}$'");
        }
    }
}
