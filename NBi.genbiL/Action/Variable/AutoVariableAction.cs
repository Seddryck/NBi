using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Variable
{
    public class AutoVariableAction : IVariableAction
    {
        public bool Value { get; private set; }
        public DateTime Now { get; private set; }

        public AutoVariableAction(bool value)
        {
            Value = value;
            Now = DateTime.Now;
        }

        public AutoVariableAction(bool value, DateTime now)
        {
            Value = value;
            Now = now;
        }

        public void Execute(GenerationState state)
        {
            if (Value)
            {
                (new SetVariableAction("now", String.Format($"{Now.Year}-{Now.Month:00}-{Now.Day:00}T{Now.Hour:00}:{Now.Minute:00}:{Now.Second:00}"))).Execute(state);
                (new SetVariableAction("time", Now.ToLongTimeString())).Execute(state);
                (new SetVariableAction("today", Now.Date.ToShortDateString())).Execute(state);
                (new SetVariableAction("username", Environment.UserName)).Execute(state);
            }
            else
                (new RevokeVariableAction(new[] { "now", "time", "today", "username" })).Execute(state);
        }

        public string Display
        {
            get
            {
                if (Value)
                    return string.Format($"Setting values for automatic variables.");
                else
                    return string.Format($"Revoking automatic variables.");
            }
        }
    }
}
