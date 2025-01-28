using NBi.Core.Scalar.Duration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

class SentinelHalfOpenDateTimeLoopStrategy : SentinelCloseDateTimeLoopStrategy
{
    public SentinelHalfOpenDateTimeLoopStrategy(DateTime seed, DateTime terminal, IDuration step)
        : base(seed, terminal, step)
    { }

    public override bool IsOngoing() => (CurrentValue < Terminal && FirstLoop) || (GetNextValue(CurrentValue, Step) < Terminal);
}
