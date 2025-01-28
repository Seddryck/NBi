using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

class SentinelHalfOpenNumericLoopStrategy : SentinelCloseNumericLoopStrategy
{
    public SentinelHalfOpenNumericLoopStrategy(decimal seed, decimal terminal, decimal step)
        : base(seed, terminal, step)
    { }

    public override bool IsOngoing() => (CurrentValue < Terminal && FirstLoop) || (GetNextValue(CurrentValue, Step) < Terminal);
}
