using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace NBi.Core.Process
{
    class WaitCommand : IDecorationCommandImplementation
    {
        private readonly int milliSeconds;

        public WaitCommand(IWaitCommand command)
		{
            milliSeconds = command.MilliSeconds;
		}

        public void Execute()
        {
            Console.WriteLine("Start waiting for {0} milli-seconds", milliSeconds);
            Thread.Sleep(milliSeconds);
            Console.WriteLine("Done with waiting {0} milli-seconds", milliSeconds);
        }
    }
}
