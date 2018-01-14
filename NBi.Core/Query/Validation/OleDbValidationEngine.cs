using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace NBi.Core.Query.Validation
{
    [SupportedCommandType(typeof(OleDbCommand))]
    internal class OleDbValidationEngine : DbCommandValidationEngine
    {
        protected internal OleDbValidationEngine(OleDbConnection connection, OleDbCommand command)
            : base(connection, command)
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
            catch (OleDbException ex)
            { throw new ConnectionException(ex, connectionString); }

            command.Connection = connection;
        }

        protected override IDbConnection NewConnection(string connectionString) => new OleDbConnection(connectionString);
    }
}
