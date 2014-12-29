using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class QueryEngineFactory
    {

        #region Parser
        /// <summary>
        /// Get an engine to parse a query. The engine returned is based on the type of the command.
        /// </summary>
        /// <param name="cmd">The command to parse</param>
        /// <returns>An engine able to parse the query</returns>
        public virtual IQueryParser GetParser(IDbCommand cmd)
        {
            return (IQueryParser)Get(cmd);
        }

        /// <summary>
        /// Get an engine to parse a query. The engine returned is based on the type of the connectionString.
        /// </summary>
        /// <param name="query">The query statement to parse</param>
        /// <param name="connectionString">The connectionString that will be used to parse this query</param>
        /// <returns>An engine able to parse the query based on the connectionString</returns>
        public virtual IQueryParser GetParser(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryParser)Get(cmd);
        }
        #endregion

        #region Performance
        /// <summary>
        /// Get an engine to monitor the performance of a query. The engine returned is based on the type of the command.
        /// </summary>
        /// <param name="cmd">The command to monitor</param>
        /// <returns>An engine able to parse the query</returns>
        public virtual IQueryPerformance GetPerformance(IDbCommand cmd)
        {
            return (IQueryPerformance)Get(cmd);
        }

        /// <summary>
        /// Get an engine to monitor the performance a query. The engine returned is based on the type of the connectionString.
        /// </summary>
        /// <param name="query">The query statement to parse</param>
        /// <param name="connectionString">The connectionString that will be used to parse this query</param>
        /// <returns>An engine able to parse the query based on the connectionString</returns>
        public virtual IQueryPerformance GetPerformance(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryPerformance)Get(cmd);
        }
        #endregion

        #region Executor
        /// <summary>
        /// Get an engine to execute and retrieve the result of a query. The engine returned is based on the type of the command.
        /// </summary>
        /// <param name="cmd">The command to execute and generating a result</param>
        /// <returns>An engine able to execute and return the resuly of the query</returns>
        public virtual IQueryExecutor GetExecutor(IDbCommand cmd)
        {
            return (IQueryExecutor)Get(cmd);
        }

        /// <summary>
        /// Get an engine to execute and retrieve the result of a query. The engine returned is based on the type of the connectionString.
        /// </summary>
        /// <param name="query">The query statement  to execute and generating a result</param>
        /// <param name="connectionString">The connectionString that will be used to parse this query</param>
        /// <returns>An engine able to execute and return the resuly of the query based on the connectionString</returns>
        public virtual IQueryExecutor GetExecutor(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryExecutor)Get(cmd);
        }
        #endregion

        #region Executor
        /// <summary>
        /// Get an engine to execute and retrieve format of the result of a query. The engine returned is based on the type of the command.
        /// </summary>
        /// <param name="cmd">The command to execute and generating a result</param>
        /// <returns>An engine able to execute and return the resuly of the query</returns>
        public IQueryFormat GetFormat(IDbCommand cmd)
        {
            return (IQueryFormat)Get(cmd);
        }

        /// <summary>
        /// Get an engine to execute and retrieve format of the result of a query. The engine returned is based on the type of the connectionString.
        /// </summary>
        /// <param name="query">The query statement  to execute and generating a result</param>
        /// <param name="connectionString">The connectionString that will be used to parse this query</param>
        /// <returns>An engine able to execute and return the resuly of the query based on the connectionString</returns>
        public virtual IQueryFormat GetFormat(string query, string connectionString)
        {
            var cmd = BuildCommand(query, connectionString);
            return (IQueryFormat)Get(cmd);
        }
        #endregion

        /// <summary>
        /// Retrieve the engine on base of the type of the command
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected IQueryEnginable Get(IDbCommand cmd)
        {
            if (cmd.GetType() == typeof(SqlCommand))
                return (IQueryEnginable)new QuerySqlEngine((SqlCommand)cmd);
            else if (cmd.GetType() == typeof(OleDbCommand))
                return (IQueryEnginable)new QueryOleDbEngine((OleDbCommand)cmd);
            else if (cmd.GetType() == typeof(AdomdCommand))
                return (IQueryEnginable)new QueryAdomdEngine((AdomdCommand)cmd);

            throw new ArgumentException();
        }

        /// <summary>
        /// On base of a query and a connectionString build an ICommand
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected IDbCommand BuildCommand(string query, string connectionString)
        {
            var conn = new ConnectionFactory().Get(connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            return cmd;
        }

        
    }
}
