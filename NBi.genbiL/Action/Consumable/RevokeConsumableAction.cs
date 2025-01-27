using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Consumable;

public class RevokeConsumableAction : IConsumableAction
{
    public IReadOnlyCollection<string> Names { get; private set; }

    public RevokeConsumableAction(IEnumerable<string> names)
    {
        Names = (IReadOnlyCollection<string>)names;
    }

    public void Execute(GenerationState state)
    {
        foreach (var name in Names)
        {
            if (state.Consumables.ContainsKey(name))
                state.Consumables.Remove(name);
        }
    }

    public string Display
    {
        get
        {
            if (Names.Count>1)
                return string.Format($"Revoking consumables '{string.Join("', '", Names)}'.");
            else
                return string.Format($"Revoking consumable '{Names.ElementAt(0)}'.");
        }
    }
}
