using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the System.Data.OleDb namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    [SupportedCommandType(typeof(OleDbCommand))]
    internal class OleDbExecutionEngine : DbCommandExecutionEngine
    {
        public OleDbExecutionEngine(OleDbConnection connection, OleDbCommand command)
            : base(connection, command)
        { }
        
        internal override void OpenConnection(IDbConnection connection)
        {
            var connectionString = Command.Connection?.ConnectionString ?? throw new NullReferenceException();
            try
            { connection.ConnectionString = connectionString; }
            catch (ArgumentException ex)
            { throw new ConnectionException(ex, connectionString); }

            try
            { connection.Open(); }
            catch (OleDbException ex)
            { throw new ConnectionException(ex, connectionString); }

            Command.Connection = connection;
        }

        protected override void HandleException(Exception ex, IDbCommand command)
        {
            if (ex is OleDbException && ex.Message == "Query timeout expired")
                OnTimeout(ex, command);
            else
                throw ex;
        }

        protected internal override IDbConnection NewConnection() 
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new OleDbConnection() : throw new PlatformNotSupportedException();
        protected override IDataAdapter NewDataAdapter(IDbCommand command)
             => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new OleDbDataAdapter((OleDbCommand)command) : throw new PlatformNotSupportedException();
    }
}
