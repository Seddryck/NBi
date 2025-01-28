using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop;

class CountNumericLoopStrategy : CountLoopStrategy<decimal, decimal>
{
    public CountNumericLoopStrategy(int count, decimal seed, decimal step)
        : base(count, seed, step)
    { }

    protected override decimal GetNextValue(decimal previousValue, decimal step) => previousValue + step;
}
