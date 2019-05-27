using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process.Commands
{
    class RunCommand : IDecorationCommand
    {
		private readonly IRunCommandArgs args;

        public RunCommand(IRunCommandArgs args) => this.args = args;

        public void Execute()
            => Execute(new PathHelper().Combine(args.BasePath, args.Path.Execute(), args.Name.Execute()), args.Argument.Execute(), args.TimeOut.Execute());

        public void Execute(string fullPath, string argument, int timeOut)
        {
            if (string.IsNullOrEmpty(argument))
                Console.WriteLine("Starting process {0} without argument.", fullPath);
            else
                Console.WriteLine("Starting process {0} with arguments \"{1}\".", fullPath, argument);
            var result = false;
            var startInfo = new ProcessStartInfo()
            {
                FileName = fullPath,
                Arguments = argument
            };

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
