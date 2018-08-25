using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Engine wrapping the System.Data.SqlClient namespace for execution of NBi tests
    /// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
    /// </summary>
    internal abstract class DbCommandExecutionEngine : IExecutionEngine
    {
        protected readonly IDbCommand command;
        private readonly Stopwatch stopWatch = new Stopwatch();
        protected internal TimeSpan CommandTimeout { get; internal set; }
        protected internal string ConnectionString { get; private set; }

        protected DbCommandExecutionEngine(IDbConnection connection, IDbCommand command)
        {
            this.command = command;
            this.CommandTimeout = new TimeSpan(0, 0, command.CommandTimeout);
            this.ConnectionString = connection.ConnectionString;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet Execute()
        {
            using (var connection = NewConnection())
            {
                OpenConnection(connection);
                InitializeCommand(command, CommandTimeout, command.Parameters, connection);
                StartWatch();
                var ds = OnExecuteDataSet(command);
                StopWatch();
                return ds;
            }
        }

        protected virtual DataSet OnExecuteDataSet(IDbCommand command)
        {
            var adapter = NewDataAdapter(command);
            var ds = new DataSet();

            try
            { adapter.Fill(ds); }
            catch (Exception ex)
            { HandleException(ex, command); }

            return ds;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalar()
        {
            using (var connection = NewConnection())
            {
                OpenConnection(connection);
                InitializeCommand(command, CommandTimeout, command.Parameters, connection);
                StartWatch();
                var value = OnExecuteScalar(command);
                StopWatch();
                return value;
            }
        }

        protected virtual object OnExecuteScalar(IDbCommand command)
        {
            object value = null;
            try
            { value = command.ExecuteScalar(); }
            catch (Exception ex)
            { HandleException(ex, command); }
            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public IEnumerable<T> ExecuteList<T>()
        {
            using (var connection = NewConnection())
            {
                OpenConnection(connection);
                InitializeCommand(command, CommandTimeout, command.Parameters, connection);
                StartWatch();
                var list = OnExecuteList<T>(command);
                StopWatch();
                return list;
            }
        }

        protected virtual IEnumerable<T> OnExecuteList<T>(IDbCommand command)
        {
            var list = new List<T>();
            try
            {
                var dr = command.ExecuteReader();
                while (dr.Read())
                    list.Add((T)dr.GetValue(0));
            }
            catch (Exception ex)
            { HandleException(ex, command); }
            return list;
        }

        internal abstract void OpenConnection(IDbConnection connection);

        protected void InitializeCommand(IDbCommand command, TimeSpan commandTimeout, IDataParameterCollection parameters, IDbConnection connection)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, command.CommandText);
            command.Connection = connection;
            foreach (IDataParameter param in parameters)
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("{0} => {1}", param.ParameterName, param.Value));
            command.CommandTimeout = Convert.ToInt32(commandTimeout.TotalSeconds);
        }

        protected abstract void HandleException(Exception ex, IDbCommand command);
        internal Action<Exception, IDbCommand> OnTimeout = (ex, command) => throw new CommandTimeoutException(ex, command);

        protected internal abstract IDbConnection NewConnection();
        protected abstract IDataAdapter NewDataAdapter(IDbCommand command);

        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Time needed to execute query: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }

        protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }
        
    }
}
