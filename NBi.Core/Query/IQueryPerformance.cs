using System.Data;

namespace NBi.Core.Query
{
    /// <summary>
    /// Interface defining methods implemented by engines able to monitor the performances of the queries
    /// </summary>
    public interface IQueryPerformance : IQueryEnginable
    {
        PerformanceResult CheckPerformance();
        PerformanceResult CheckPerformance(int timeout);
        void CleanCache();
    }
}
