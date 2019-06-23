using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process.Commands
{
    class KillCommand : IDecorationCommand
    {
        private readonly IKillCommandArgs args;

        public KillCommand(IKillCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.ProcessName.Execute());

        internal void Execute(string processName)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);

            if (processes == null || processes.Count() == 0)
                Console.WriteLine($"No process named '{processName}' to kill.");

            foreach (var process in processes)
            {
                process.Kill();
                Console.WriteLine($"Process named '{processName}' killed.");
            }
        }
    }
}
