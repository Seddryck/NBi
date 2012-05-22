using System.Data;

namespace NBi.Core.ResultSet
{
    public interface IResultSetComparer
    {
        Result Validate(IDbCommand path);
    }
}
