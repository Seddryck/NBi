using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace NBi.Core.Decoration.Process.Commands;

class WaitCommand : IDecorationCommand
{
    private readonly IScalarResolver<int> milliSeconds;

    public WaitCommand(WaitCommandArgs args)
		{
        milliSeconds = args.MilliSeconds;
		}

    public void Execute()
    {
        var ms = milliSeconds.Execute();
        Console.WriteLine($"Start waiting for {ms} milli-seconds");
        Thread.Sleep(ms);
        Console.WriteLine($"Done with waiting {ms} milli-seconds");
    }
}
