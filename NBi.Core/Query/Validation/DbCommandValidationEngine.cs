using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace NBi.Core.Query.Validation
{
    /// <summary>
    /// Engine wrapping the System.Microsoft.SqlClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal abstract class DbCommandValidationEngine : IValidationEngine
    {
        protected readonly IDbCommand command;
        private readonly Stopwatch stopWatch = new Stopwatch();

        protected DbCommandValidationEngine(IDbConnection connection, IDbCommand command)
        {
            this.command = command;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual ParserResult Parse()
        {
            ParserResult? res = null;

            using (var connection = NewConnection(command.Connection!.ConnectionString))
            {
                var fullSql = $@"SET FMTONLY ON; {command.CommandText} SET FMTONLY OFF;";
                OpenConnection(connection);
                StartWatch();
                using var cmdIn = connection.CreateCommand();
                cmdIn.CommandText = fullSql;
                InitializeCommand(cmdIn, connection);
                try
                {
                    cmdIn.ExecuteNonQuery();
                    res = ParserResult.NoParsingError();
                }
                catch (Exception ex)
                {
                    res = new ParserResult(ParseMessage(ex.Message));
                }
            }
            StopWatch();
            return res;
        }

        protected abstract void OpenConnection(IDbConnection connection);

        protected void InitializeCommand(IDbCommand command, IDbConnection connection)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, command.CommandText);
            command.Connection = connection;
            foreach (IDataParameter param in command.Parameters)
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("{0} => {1}", param.ParameterName, param.Value));
        }
        
        protected abstract IDbConnection NewConnection(string connectionString);
        protected virtual string[] ParseMessage(string message) => message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries); 

        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Time needed to parse query: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }
    }
}
