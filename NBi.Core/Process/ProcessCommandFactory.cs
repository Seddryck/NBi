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
            
            throw new ArgumentException();
        }
    }
}
