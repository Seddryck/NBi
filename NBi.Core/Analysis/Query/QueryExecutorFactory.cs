using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace NBi.Core.Analysis.Query
{
    public class QueryExecutorFactory
    {
        public static IQueryExecutor Get(IDbCommand cmd)
        {
            if (cmd.GetType() == typeof(SqlCommand))
                return new QuerySqlExecutor(cmd.Connection.ConnectionString);
            else if (cmd.GetType() == typeof(OleDbCommand))
                return new QueryOleDbExecutor(cmd.Connection.ConnectionString);

            throw new ArgumentException();
        }
    }
}
