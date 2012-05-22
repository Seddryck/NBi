using System.Data;

namespace NBi.Core.Query
{
    public interface IQueryPerformance : IQueryEnginable
    {
        PerformanceResult CheckPerformance(IDbCommand cmd, bool cleanCache);
    }
}
