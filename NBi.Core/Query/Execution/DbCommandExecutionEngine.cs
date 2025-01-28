using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution;

internal abstract class DbCommandExecutionEngine : IExecutionEngine
{
    protected IDbCommand Command { get; }
    private readonly Stopwatch stopWatch = new Stopwatch();
    public TimeSpan CommandTimeout 
    { 
        get => new TimeSpan(0, 0, Command.CommandTimeout); 
        set => Command.CommandTimeout = Convert.ToInt32(Math.Ceiling(value.TotalSeconds)); 
    }
    protected internal string ConnectionString { get; private set; }

    protected DbCommandExecutionEngine(IDbConnection connection, IDbCommand command)
    {
        Command = command;
        ConnectionString = connection.ConnectionString;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    public DataSet Execute()
    {
        using var connection = NewConnection();
        OpenConnection(connection);
        InitializeCommand(Command, CommandTimeout, Command.Parameters, connection);
        StartWatch();
        var ds = OnExecuteDataSet(Command);
        StopWatch();
        return ds;
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
    public object? ExecuteScalar()
    {
        using var connection = NewConnection();
        OpenConnection(connection);
        InitializeCommand(Command, CommandTimeout, Command.Parameters, connection);
        StartWatch();
        var value = OnExecuteScalar(Command);
        StopWatch();
        return value;
    }

    protected virtual object? OnExecuteScalar(IDbCommand command)
    {
        object? value = null;
        try
        { value = command.ExecuteScalar(); }
        catch (Exception ex)
        { HandleException(ex, command); }
        return value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
    public IEnumerable<T> ExecuteList<T>()
    {
        using var connection = NewConnection();
        OpenConnection(connection);
        InitializeCommand(Command, CommandTimeout, Command.Parameters, connection);
        StartWatch();
        var list = OnExecuteList<T>(Command);
        StopWatch();
        return list;
    }

    protected virtual IEnumerable<T> OnExecuteList<T>(IDbCommand command)
    {
        var list = new List<T>();
        try
        {
            var dr = command.ExecuteReader();
            while (dr.Read())
                list.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
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
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"{param.ParameterName} => {param.Value}");
        command.CommandTimeout = Convert.ToInt32(commandTimeout.TotalSeconds);
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Command timeout set to {command.CommandTimeout} second{(command.CommandTimeout>1 ? "s" : string.Empty)}");
    }

    protected abstract void HandleException(Exception ex, IDbCommand command);
    internal Action<Exception, IDbCommand> OnTimeout = (ex, command) => throw new CommandTimeoutException(ex, command);

    protected internal abstract IDbConnection NewConnection();
    protected abstract IDataAdapter NewDataAdapter(IDbCommand command);

    protected void StartWatch() => stopWatch.Restart();

    protected void StopWatch()
    {
        stopWatch.Stop();
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Time needed to execute query: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
    }

    protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }

}
