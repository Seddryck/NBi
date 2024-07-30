using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the System.Microsoft.SqlClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    [SupportedCommandType(typeof(SqlCommand))]
    internal class SqlExecutionEngine : DbCommandExecutionEngine
    {
        public SqlExecutionEngine(SqlConnection connection, SqlCommand command)
            : base(connection, command)
        { }
        
        internal override void OpenConnection(IDbConnection connection)
        {
            var connectionString = Command.Connection!.ConnectionString;
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
            if (ex is SqlException sqlEx && sqlEx.Number == -2)
                OnTimeout(ex, command);
            else
                throw ex;
        }

        protected internal override IDbConnection NewConnection() => new SqlConnection();
        protected override IDataAdapter NewDataAdapter(IDbCommand command) => new SqlDataAdapter((SqlCommand)command);
    }
}
