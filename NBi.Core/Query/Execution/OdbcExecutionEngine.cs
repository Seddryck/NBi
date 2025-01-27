using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution;

/// <summary>
/// Engine wrapping the System.Data.Odbc namespace for execution of NBi tests
/// <remarks>Instances of this class are built by the means of the <see>QueryEngineFactory</see></remarks>
/// </summary>
[SupportedCommandType(typeof(OdbcCommand))]
internal class OdbcExecutionEngine : DbCommandExecutionEngine
{
    public OdbcExecutionEngine(OdbcConnection connection, OdbcCommand command)
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
        catch (OdbcException ex)
        { throw new ConnectionException(ex, connectionString); }
    }

    protected override void HandleException(Exception ex, IDbCommand command)
    {
        if (ex is OdbcException && ex.Message.EndsWith("Query timeout expired"))
            OnTimeout(ex, command);
        else
            throw ex;
    }

    protected internal override IDbConnection NewConnection() => new OdbcConnection();
    protected override IDataAdapter NewDataAdapter(IDbCommand command) => new OdbcDataAdapter((OdbcCommand)command);
}

