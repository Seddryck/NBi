using System.Data;

namespace NBi.Core.Query
{
    public interface IQueryExecutor : IQueryEnginable
    {
        DataSet Execute();
    }
}
