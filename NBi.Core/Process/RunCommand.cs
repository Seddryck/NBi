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
            if (string.IsNullOrEmpty(argument))
                Console.WriteLine("Starting process {0} without argument.", fullPath);
            else
                Console.WriteLine("Starting process {0} with arguments \"{1}\".", fullPath, argument);
            var result = false;
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = fullPath;
            startInfo.Arguments = argument;
            using (var exeProcess = System.Diagnostics.Process.Start(startInfo))
            {
                if (timeOut != 0)
                {
                    Console.WriteLine("Waiting the end of the process.");
                    exeProcess.WaitForExit(timeOut);
                    if (exeProcess.HasExited)
                    {
                        if (exeProcess.ExitCode == 0)
                            Console.WriteLine("Process has been successfully executed.");
                        else
                            Console.WriteLine("Process has failed.");
                        result = exeProcess.ExitCode == 0;
                    }
                    else
                        Console.WriteLine("Process has been interrupted before the end of its execution.");
                        
                }
                else
                {
                    Console.WriteLine("Not waiting the end of the process.");
                    result = true;
                }
            }

            if (!result)
                throw new InvalidProgramException();

        }
    }
}
