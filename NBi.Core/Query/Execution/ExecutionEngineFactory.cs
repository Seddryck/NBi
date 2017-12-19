using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Connection;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory
    {
        public IExecutionEngine Instantiate(IQuery query)
        {
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Instantiate(query.ConnectionString).CreateNew() as IDbConnection;

            var commandFactory = new DbCommandFactory();
            var cmd = commandFactory.Instantiate(connection, query); 

            if (cmd is SqlCommand)
                return new SqlExecutionEngine((SqlCommand)cmd);
            else if (cmd is OleDbCommand)
                return new OleDbExecutionEngine((OleDbCommand)cmd);
            else if (cmd is AdomdCommand)
                return new AdomdExecutionEngine((AdomdCommand)cmd);
            else if (cmd is OdbcCommand)
                return new OdbcExecutionEngine((OdbcCommand)cmd);

            throw new ArgumentException();
        }

    }
}
