using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Connection;

namespace NBi.Core.Query.Format
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class FormatEngineFactory
    {
        public IFormatEngine Instantiate(IQuery query)
        {
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Instantiate(query.ConnectionString).CreateNew() as IDbConnection;

            var commandFactory = new DbCommandFactory();
            var cmd = commandFactory.Instantiate(connection, query);

            if (cmd is SqlCommand)
                return new SqlFormatEngine((SqlCommand)cmd);
            else if (cmd is OleDbCommand)
                return new OleDbFormatEngine((OleDbCommand)cmd);
            else if (cmd is AdomdCommand)
                return new AdomdFormatEngine((AdomdCommand)cmd);
            else if (cmd is OdbcCommand)
                return new OdbcFormatEngine((OdbcCommand)cmd);

            throw new ArgumentException();
        }

    }
}
