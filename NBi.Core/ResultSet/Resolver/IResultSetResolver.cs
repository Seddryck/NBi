using System.Data;

namespace NBi.Core.ResultSet.Resolver
{
    public interface IResultSetResolver
    {
        ResultSet Execute();
    }
}
