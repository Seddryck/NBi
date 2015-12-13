using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NBi.Core.Process
{
    class KillCommand : IDecorationCommandImplementation
    {
        private readonly string processName;

        public KillCommand(IKillCommand command)
		{   
            processName = command.ProcessName;
        }

        public void Execute()
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);

            if (processes == null || processes.Count() == 0)
                Console.WriteLine("No process named '{0}' to kill.", processName);

            foreach (var process in processes)
            {
                process.Kill();
                Console.WriteLine("Process named '{0}' killed.", processName);
            }
        }
    }
}
