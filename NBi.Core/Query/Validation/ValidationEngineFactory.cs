using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ValidationEngineFactory
    {
        public IValidationEngine Instantiate(IQuery query)
        {
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Instantiate(query.ConnectionString);

            var commandFactory = new DbCommandFactory();
            var cmd = commandFactory.Instantiate(connection, query);

            if (cmd is SqlCommand)
                return new SqlValidationEngine((SqlCommand)cmd);
            else if (cmd is OleDbCommand)
                return new OleDbValidationEngine((OleDbCommand)cmd);
            else if (cmd is AdomdCommand)
                return new AdomdValidationEngine((AdomdCommand)cmd);
            else if (cmd is OdbcCommand)
                return new OdbcValidationEngine((OdbcCommand)cmd);

            throw new ArgumentException();
        }

    }
}
