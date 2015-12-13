using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Process
{
    class ProcessCommandFactory
    {
        public IDecorationCommandImplementation Get(IProcessCommand command)
        {
            if (command is IRunCommand)
                return new RunCommand(command as IRunCommand);
            if (command is IKillCommand)
                return new KillCommand(command as IKillCommand);
            if (command is IWaitCommand)
                return new WaitCommand(command as IWaitCommand);
            
            throw new ArgumentException();
        }
    }
}
