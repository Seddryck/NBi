using System.Data;

namespace NBi.Core.Analysis.Query
{
    public interface IResultSetComparer
    {
        Result Validate(IDbCommand path);
    }
}
