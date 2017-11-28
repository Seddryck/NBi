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
    internal abstract class DbCommandExecutionEngine : IExecutionEngine
    {
        protected readonly IDbCommand command;
        private readonly Stopwatch stopWatch = new Stopwatch();

        protected DbCommandExecutionEngine(IDbCommand command)
        {
            this.command = command;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet Execute()
        {
            using (var connection = NewConnection())
            {
                OpenConnection(connection);
                InitializeCommand(command, connection);
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
                InitializeCommand(command, connection);
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
                InitializeCommand(command, connection);
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

        protected abstract void OpenConnection(IDbConnection connection);

        private void InitializeCommand(IDbCommand command, IDbConnection connection)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, command.CommandText);
            command.Connection = connection;
            foreach (IDataParameter param in command.Parameters)
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("{0} => {1}", param.ParameterName, param.Value));
        }

        protected abstract void HandleException(Exception ex, IDbCommand command);

        protected abstract IDbConnection NewConnection();
        protected abstract IDataAdapter NewDataAdapter(IDbCommand command);

        private void StartWatch()
        {
            stopWatch.Restart();
        }

        private void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to execute query: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }
    }
}
