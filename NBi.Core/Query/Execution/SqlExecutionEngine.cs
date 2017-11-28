using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the System.Data.SqlClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal class SqlExecutionEngine : DbCommandExecutionEngine
    {
        protected internal SqlExecutionEngine(IDbCommand command)
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
            catch (SqlException ex)
            { throw new ConnectionException(ex, connectionString); }
        }

        protected override void HandleException(Exception ex, IDbCommand command)
        {
            if (ex is SqlException && (ex as SqlException).Number == -2)
                throw new CommandTimeoutException(ex, command);
            throw ex;
        }

        protected override IDbConnection NewConnection() => new SqlConnection();
        protected override IDataAdapter NewDataAdapter(IDbCommand command) => new SqlDataAdapter((SqlCommand)command);
    }
}
