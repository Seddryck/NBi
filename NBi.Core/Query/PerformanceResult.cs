using System;

namespace NBi.Core.Query
{
    public class PerformanceResult
    {
        public TimeSpan TimeElapsed { get; set; }

        public PerformanceResult(TimeSpan timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }
    }
}
