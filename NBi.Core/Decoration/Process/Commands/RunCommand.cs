using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process.Commands;

class RunCommand : IDecorationCommand
{
		private readonly ProcessRunCommandArgs args;

    public RunCommand(ProcessRunCommandArgs args) => this.args = args;

    public void Execute()
        => Execute(PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute() ?? string.Empty, args.Name.Execute() ?? string.Empty), args.Argument.Execute() ?? string.Empty, args.TimeOut.Execute());

    public void Execute(string fullPath, string argument, int timeOut)
    {
        if (string.IsNullOrEmpty(argument))
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Starting process {fullPath} without argument.");
        else
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Starting process {fullPath} with arguments \"{argument}\".");

        var startInfo = new ProcessStartInfo()
        {
            FileName = fullPath,
            Arguments = argument
        };


        using var exeProcess = System.Diagnostics.Process.Start(startInfo) ?? throw new NullReferenceException();
        if (timeOut != 0)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "Waiting the end of the process.");
            exeProcess.WaitForExit(timeOut);
            if (exeProcess.HasExited)
            {
                if (exeProcess.ExitCode == 0)
                    Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, "Process has been successfully executed.");
                else
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, "Process has failed.");
                    throw new NBiException($"Process has failed and returned an exit code '{exeProcess.ExitCode}'.");
                }
            }
            else
                throw new NBiException($"Process has been interrupted before the end of its execution.");

        }
        else
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, "Not waiting the end of the process.");
        }
    }
}
