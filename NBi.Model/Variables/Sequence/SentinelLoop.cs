using NBi.Core.Sequence.Resolver.Loop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables.Sequence;

public class SentinelLoop
{
    public string Seed { get; set; }

    public string Terminal { get; set; }

    public string Step { get; set; }

    public IntervalMode IntervalMode { get; set; }

    public SentinelLoop()
    {
        IntervalMode = IntervalMode.Close;
    }
}
