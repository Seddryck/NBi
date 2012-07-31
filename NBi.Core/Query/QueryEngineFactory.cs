using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    public class QueryEngineFactory
    {

        #region Parser
        public virtual IQueryParser GetParser(IDbCommand cmd)
        {
            return (IQueryParser)Get(cmd);
        }

        public virtual IQueryParser GetParser(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryParser)Get(cmd);
        }
        #endregion

        #region Performance
        public virtual IQueryPerformance GetPerformance(IDbCommand cmd)
        {
            return (IQueryPerformance)Get(cmd);
        }

        public virtual IQueryPerformance GetPerformance(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryPerformance)Get(cmd);
        }
        #endregion

        #region Executor
        public virtual IQueryExecutor GetExecutor(IDbCommand cmd)
        {
            return (IQueryExecutor)Get(cmd);
        }

        public virtual IQueryExecutor GetExecutor(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryExecutor)Get(cmd);
        }
        #endregion

        protected static IQueryEnginable Get(IDbCommand cmd)
        {
            if (cmd.GetType() == typeof(SqlCommand))
                return (IQueryEnginable)new QuerySqlEngine((SqlCommand)cmd);
            else if (cmd.GetType() == typeof(OleDbCommand))
                return (IQueryEnginable)new QueryOleDbEngine((OleDbCommand)cmd);
            else if (cmd.GetType() == typeof(AdomdCommand))
                return (IQueryEnginable)new QueryAdomdEngine((AdomdCommand)cmd);

            throw new ArgumentException();
        }

        protected static IDbCommand BuildCommand(string query, string connectionString)
        {
            var conn = ConnectionFactory.Get(connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            return cmd;
        }
    }
}
