using NBi.Core.Decoration.Process.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process
{
    class ProcessCommandFactory
    {
        public IDecorationCommand Instantiate(IProcessCommandArgs args)
        {
            return args switch
            {
                ProcessRunCommandArgs runArgs => new RunCommand(runArgs),
                ProcessKillCommandArgs killArgs => new KillCommand(killArgs),
                WaitCommandArgs waitArgs => new WaitCommand(waitArgs),
                ServiceStartCommandArgs startArgs => new StartCommand(startArgs),
                ServiceStopCommandArgs stopArgs => new StopCommand(stopArgs),
                _ => throw new ArgumentException(),
            };
        }
    }
}
