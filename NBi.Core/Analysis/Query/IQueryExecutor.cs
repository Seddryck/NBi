using System.Data;

namespace NBi.Core.Analysis.Query
{
    public interface IQueryExecutor
    {
        DataSet Execute(string mdx);
    }
}
