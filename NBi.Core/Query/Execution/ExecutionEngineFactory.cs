using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory
    {
        public IExecutionEngine Instantiate(IDbCommand cmd)
        {
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
