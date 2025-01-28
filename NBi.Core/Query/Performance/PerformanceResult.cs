using System;

namespace NBi.Core.Query.Performance;

public class PerformanceResult
{
    public TimeSpan TimeElapsed { get; private set; }

    public TimeSpan TimeOut { get; private set; }

    public bool IsTimeOut { get; private set; }

    public PerformanceResult(TimeSpan timeElapsed)
    {
        TimeElapsed = timeElapsed;
        IsTimeOut = false;
    }

    internal PerformanceResult()
    {
    }

    internal static PerformanceResult Timeout(TimeSpan timeout)
    {
        return new PerformanceResult() {IsTimeOut=true, TimeOut = timeout};
    }
}
