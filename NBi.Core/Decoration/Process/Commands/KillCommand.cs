using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process.Commands;

class KillCommand : IDecorationCommand
{
    private readonly ProcessKillCommandArgs args;

    public KillCommand(ProcessKillCommandArgs args) => this.args = args;

    public void Execute() => Execute(args.ProcessName.Execute() ?? throw new NullReferenceException());

    internal void Execute(string processName)
    {
        var processes = System.Diagnostics.Process.GetProcessesByName(processName);

        if (processes.Length == 0)
            Console.WriteLine($"No process named '{processName}' to kill.");

        foreach (var process in processes)
        {
            process.Kill();
            Console.WriteLine($"Process named '{processName}' killed.");
        }
    }
}
