using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Duration;

public class FixedDuration : IDuration
{
    public TimeSpan TimeSpan { get; }

    public FixedDuration(TimeSpan timeSpan)
    {
        TimeSpan = timeSpan;
    }
}
