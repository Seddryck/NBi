using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

class MonthDuration : IDuration
{
    public int Count { get; }

    public MonthDuration(int count)
    {
        Count = count;
    }
}
