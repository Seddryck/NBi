using NBi.Core.Decoration.Process.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process;

class ProcessConditionFactory
{
    public IDecorationCondition Instantiate(IProcessConditionArgs args)
    {
        return args switch
        {
            IRunningConditionArgs runningArgs => new RunningCondition(runningArgs),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
