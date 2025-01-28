using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

class YearDuration : IDuration
{
    public int Count { get; }

    public YearDuration(int count)
    {
        Count = count;
    }
}
