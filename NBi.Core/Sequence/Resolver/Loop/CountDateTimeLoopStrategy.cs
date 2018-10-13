using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver.Loop
{
    class CountDateTimeLoopStrategy : CountLoopStrategy<DateTime, TimeSpan>
    {
        public CountDateTimeLoopStrategy(int count, DateTime seed, TimeSpan step)
            : base(count, seed, step)
        { }

        protected override DateTime GetNextValue(DateTime previousValue, TimeSpan step) => previousValue.Add(step);
    }
}
