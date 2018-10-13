using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop
{
    class SentinelDateTimeLoopStrategy : SentinelLoopStrategy<DateTime, TimeSpan>
    {
        public SentinelDateTimeLoopStrategy(DateTime seed, DateTime terminal, TimeSpan step)
            : base(seed, terminal, step)
        { }

        protected override DateTime GetNextValue(DateTime previousValue, TimeSpan step) => previousValue.Add(step);
        public override bool IsOngoing() => (CurrentValue <= Terminal && FirstLoop) || (GetNextValue(CurrentValue, Step) <= Terminal);
    }
}
