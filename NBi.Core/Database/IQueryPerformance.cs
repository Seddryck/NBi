using System.Data;

namespace NBi.Core.Database
{
    public interface IQueryPerformance
    {
        Result Validate(IDbCommand cmd);
    }
}
