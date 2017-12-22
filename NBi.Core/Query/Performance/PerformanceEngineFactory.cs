using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Command;
using NBi.Core.Query.Session;

namespace NBi.Core.Query.Performance
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class PerformanceEngineFactory
    {
        public IPerformanceEngine Instantiate(IQuery query)
        {
            var sessionFactory = new SessionFactory();
            var session = sessionFactory.Instantiate(query.ConnectionString);

            var commandFactory = new CommandFactory();
            var cmd = commandFactory.Instantiate(session, query).Implementation;

            if (cmd is SqlCommand)
                return new SqlPerformanceEngine((SqlCommand)cmd);
            else if (cmd is OleDbCommand)
                return new OleDbPerformanceEngine((OleDbCommand)cmd);
            else if (cmd is AdomdCommand)
                return new AdomdPerformanceEngine((AdomdCommand)cmd);
            else if (cmd is OdbcCommand)
                return new OdbcPerformanceEngine((OdbcCommand)cmd);

            throw new ArgumentException();
        }

    }
}
