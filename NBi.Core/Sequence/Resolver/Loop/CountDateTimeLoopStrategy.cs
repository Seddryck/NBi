using NBi.Core.Scalar.Duration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

class CountDateTimeLoopStrategy : CountLoopStrategy<DateTime, IDuration>
{
    public CountDateTimeLoopStrategy(int count, DateTime seed, IDuration step)
        : base(count, seed, step)
    { }

    protected override DateTime GetNextValue(DateTime previousValue, IDuration step) => previousValue.Add(step);
}
