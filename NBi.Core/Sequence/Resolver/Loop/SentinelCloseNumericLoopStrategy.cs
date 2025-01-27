using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

class SentinelCloseNumericLoopStrategy : SentinelLoopStrategy<decimal, decimal>
{
    public SentinelCloseNumericLoopStrategy(decimal seed, decimal terminal, decimal step)
        : base(seed, terminal, step)
    { }

    protected override decimal GetNextValue(decimal previousValue, decimal step) => previousValue + step;
    public override bool IsOngoing() => (CurrentValue <= Terminal && FirstLoop) || (GetNextValue(CurrentValue, Step) <= Terminal);
}
