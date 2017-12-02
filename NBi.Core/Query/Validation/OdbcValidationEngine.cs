using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Engine wrapping the System.Data.Odbc namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal class OdbcValidationEngine : DbCommandValidationEngine
    {
        protected internal OdbcValidationEngine(OdbcCommand command)
            : base(command)
        { }
        
        protected override void OpenConnection(IDbConnection connection)
        {
            var connectionString = command.Connection.ConnectionString;
            try
            { connection.ConnectionString = connectionString; }
            catch (ArgumentException ex)
            { throw new ConnectionException(ex, connectionString); }

            try
            { connection.Open(); }
            catch (OdbcException ex)
            { throw new ConnectionException(ex, connectionString); }
        }

        protected override string[] ParseMessage(string message) => message.Split(new string[] { "\r\n", "[SQL Server]" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !x.EndsWith("SQL Server]")).ToArray();

        protected override IDbConnection NewConnection(string connectionString) => new OdbcConnection(connectionString);
    }
}

