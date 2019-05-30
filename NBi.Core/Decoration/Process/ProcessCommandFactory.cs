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
                case IRunCommandArgs runArgs: return new RunCommand(runArgs);
                case IKillCommandArgs killArgs: return new KillCommand(killArgs);
                case IWaitCommandArgs waitArgs: return new WaitCommand(waitArgs);
                case IStartCommandArgs startArgs: return new StartCommand(startArgs);
                case IStopCommandArgs stopArgs: return new StopCommand(stopArgs);
                default: throw new ArgumentException();
            }
        }
    }
}
