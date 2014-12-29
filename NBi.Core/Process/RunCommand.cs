using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NBi.Core.Process
{
    class RunCommand : IDecorationCommandImplementation
    {
		private readonly string argument;
        private readonly string fullPath;
        private readonly int timeOut;

        public RunCommand(IRunCommand command)
		{
            argument = command.Argument;
            fullPath = command.FullPath;
            timeOut = command.TimeOut;
		}

        public void Execute()
        {
            var result = false;
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = fullPath;
            startInfo.Arguments = argument;
            using (var exeProcess = System.Diagnostics.Process.Start(startInfo))
            {
                if (timeOut != 0)
                {
                    exeProcess.WaitForExit(timeOut);
                    if (exeProcess.HasExited)
                        result = exeProcess.ExitCode == 0;
                }
                else
                    result = true;
                
            }

            if (!result)
                throw new InvalidProgramException();

        }
    }
}
