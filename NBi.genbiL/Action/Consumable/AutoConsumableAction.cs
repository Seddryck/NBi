using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Consumable;

public class AutoConsumableAction : IConsumableAction
{
    public bool Value { get; private set; }
    public DateTime Now { get; private set; }

    public AutoConsumableAction(bool value)
    {
        Value = value;
        Now = DateTime.Now;
    }

    public AutoConsumableAction(bool value, DateTime now)
    {
        Value = value;
        Now = now;
    }

    public void Execute(GenerationState state)
    {
        if (Value)
        {
            (new SetConsumableAction("now", String.Format($"{Now.Year}-{Now.Month:00}-{Now.Day:00}T{Now.Hour:00}:{Now.Minute:00}:{Now.Second:00}"))).Execute(state);
            (new SetConsumableAction("time", Now.ToLongTimeString())).Execute(state);
            (new SetConsumableAction("today", Now.Date.ToShortDateString())).Execute(state);
            (new SetConsumableAction("username", Environment.UserName)).Execute(state);
        }
        else
            (new RevokeConsumableAction(new[] { "now", "time", "today", "username" })).Execute(state);
    }

    public string Display
    {
        get
        {
            if (Value)
                return string.Format($"Setting values for automatic consumables.");
            else
                return string.Format($"Revoking automatic consumables.");
        }
    }
}
