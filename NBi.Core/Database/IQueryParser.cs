using System.Data;

namespace NBi.Core.Database
{
    public interface IQueryParser
    {
        Result Validate(IDbCommand cmd);
    }
}
