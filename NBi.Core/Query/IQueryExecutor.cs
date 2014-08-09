using System.Data;

namespace NBi.Core.Query
{
    /// <summary>
    /// Interface defining methods implemented by engines able to execute queries and retrieve the result
    /// </summary>
    public interface IQueryExecutor : IQueryEnginable
    {
        DataSet Execute();
    }
}
