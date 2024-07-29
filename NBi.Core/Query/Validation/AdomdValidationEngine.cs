using System;
using System.Data;
using System.Diagnostics;
using Microsoft.AnalysisServices.AdomdClient;
using System.IO;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Engine wrapping the Microsoft.AnalysisServices.AdomdClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>ExecutionEngineFactory</see></remarks>
    /// </summary>
    [SupportedCommandType(typeof(AdomdCommand))]
    internal class AdomdValidationEngine : DbCommandValidationEngine
    {
        public AdomdValidationEngine(AdomdConnection connection, AdomdCommand command)
            : base(connection, command)
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public override ParserResult Parse()
        {
            ParserResult? res = null;

            using (var connection = NewConnection(command.Connection!.ConnectionString))
            {
                OpenConnection(connection);
                StartWatch();
                InitializeCommand(command, connection);
                try
                {
                    command.ExecuteReader(CommandBehavior.SchemaOnly);
                    res = ParserResult.NoParsingError();
                }
                catch (AdomdException ex)
                {
                    res = new ParserResult(ex.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            StopWatch();
            return res;
        }

        protected override void OpenConnection(IDbConnection connection)
        {
            var connectionString = command.Connection!.ConnectionString;
            try
            { connection.ConnectionString = connectionString; }
            catch (ArgumentException ex)
            { throw new ConnectionException(ex, connectionString); }

            try
            { connection.Open(); }
            catch (Exception ex)
            { throw new ConnectionException(ex, connectionString); }
        }

        protected override IDbConnection NewConnection(string connectionString) => new AdomdConnection(connectionString);
    }
}
