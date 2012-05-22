using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace NBi.Core.Query
{
    public class QueryEngineFactory
    {
        public static IQueryEnginable Get(IDbCommand cmd)
        {
            if (cmd.GetType() == typeof(SqlCommand))
                return (IQueryEnginable) new QuerySqlEngine(cmd.Connection.ConnectionString);
            else if (cmd.GetType() == typeof(OleDbCommand))
                return (IQueryEnginable) new QueryOleDbEngine(cmd.Connection.ConnectionString);

            throw new ArgumentException();
        }
    }
}
