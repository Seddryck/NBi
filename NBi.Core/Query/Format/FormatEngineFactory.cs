using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Command;
using NBi.Core.Query.Session;

namespace NBi.Core.Query.Format
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class FormatEngineFactory
    {
        public IFormatEngine Instantiate(IQuery query)
        {
            var sessionFactory = new SessionFactory();
            var session = sessionFactory.Instantiate(query.ConnectionString);

            var commandFactory = new CommandFactory();
            var cmd = commandFactory.Instantiate(session, query);

            if (cmd.Implementation is SqlCommand)
                return new SqlFormatEngine((SqlCommand)cmd.Implementation);
            else if (cmd.Implementation is OleDbCommand)
                return new OleDbFormatEngine((OleDbCommand)cmd.Implementation);
            else if (cmd.Implementation is AdomdCommand)
                return new AdomdFormatEngine((AdomdCommand)cmd.Implementation);
            else if (cmd.Implementation is OdbcCommand)
                return new OdbcFormatEngine((OdbcCommand)cmd.Implementation);

            throw new ArgumentException();
        }

    }
}
