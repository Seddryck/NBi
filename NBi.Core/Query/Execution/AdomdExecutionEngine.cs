using System;
using System.Data;
using System.Diagnostics;
using Microsoft.AnalysisServices.AdomdClient;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the Microsoft.AnalysisServices.AdomdClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>ExecutionEngineFactory</see></remarks>
    /// </summary>
    [SupportedCommandType(typeof(AdomdCommand))]
    internal class AdomdExecutionEngine : DbCommandExecutionEngine
    {
        public AdomdExecutionEngine(AdomdConnection connection, AdomdCommand command)
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
            catch (Exception ex)
            { throw new ConnectionException(ex, connectionString); }
        }

        protected override void HandleException(Exception ex, IDbCommand command) 
        {
            if (ex is AdomdConnectionException)
                throw new ConnectionException(ex, command.Connection!.ConnectionString);
            if (ex is AdomdErrorResponseException && ex.Message.StartsWith("Timeout expired."))
                OnTimeout(ex, command);
            throw ex;
        }

        protected internal override IDbConnection NewConnection() => new AdomdConnection();
        protected override IDataAdapter NewDataAdapter(IDbCommand command) => new AdomdDataAdapter((AdomdCommand)command);
    }
}
