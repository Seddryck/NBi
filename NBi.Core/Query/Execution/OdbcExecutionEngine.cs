using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the System.Data.Odbc namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal class OdbcExecutionEngine : DbCommandExecutionEngine
    {
        protected internal OdbcExecutionEngine(OdbcCommand command)
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

        protected override void HandleException(Exception ex, IDbCommand command)
        {
            if (ex is OdbcException && ex.Message.EndsWith("Query timeout expired"))
                throw new CommandTimeoutException(ex, command);
            throw ex;
        }

        protected override IDbConnection NewConnection() => new OdbcConnection();
        protected override IDataAdapter NewDataAdapter(IDbCommand command) => new OdbcDataAdapter((OdbcCommand)command);
    }
}

