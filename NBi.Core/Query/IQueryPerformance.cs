using System.Data;

namespace NBi.Core.Query
{
    public interface IQueryPerformance : IQueryEnginable
    {
        PerformanceResult CheckPerformance();
        void CleanCache();
    }
}
