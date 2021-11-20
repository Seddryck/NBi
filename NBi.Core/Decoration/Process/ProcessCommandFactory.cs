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
            switch (args)
            {
                case ProcessRunCommandArgs runArgs: return new RunCommand(runArgs);
                case ProcessKillCommandArgs killArgs: return new KillCommand(killArgs);
                case WaitCommandArgs waitArgs: return new WaitCommand(waitArgs);
                case ServiceStartCommandArgs startArgs: return new StartCommand(startArgs);
                case ServiceStopCommandArgs stopArgs: return new StopCommand(stopArgs);
                default: throw new ArgumentException();
            }
        }
    }
}
