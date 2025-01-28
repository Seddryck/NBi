using System;
using System.Collections.Generic;
using System.Data;

namespace NBi.Core.Query.Performance;

/// <summary>
/// Interface defining methods implemented by engines able to execute queries and retrieve the result
/// </summary>
public interface IPerformanceEngine
{
    PerformanceResult Execute();
    PerformanceResult Execute(TimeSpan timeout);
    void CleanCache();
}
