using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action;

class EmptyAction : IAction
{

    public void Execute(GenerationState state)
    {
        return;
    }

    public string Display
    {
        get
        {
            return string.Empty;
        }
    }
}
