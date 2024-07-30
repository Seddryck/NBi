using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Engine wrapping the System.Microsoft.SqlClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    [SupportedCommandType(typeof(SqlCommand))]
    internal class SqlValidationEngine : DbCommandValidationEngine
    {
        public SqlValidationEngine(SqlConnection connection, SqlCommand command)
            : base(connection, command)
        { }

        protected override void OpenConnection(IDbConnection connection)
        {
            var connectionString = command.Connection!.ConnectionString;
            try
            { connection.ConnectionString = connectionString; }
            catch (ArgumentException ex)
            { throw new ConnectionException(ex, connectionString); }
            try
            { connection.Open(); }
            catch (SqlException ex)
            { throw new ConnectionException(ex, connectionString); }
        }

        protected override IDbConnection NewConnection(string connectionString) => new SqlConnection(connectionString);
    }
}
