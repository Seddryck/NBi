using NBi.Core.Query.Client;
using NBi.Extensibility.Query;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace NBi.Core.Query.Execution;

internal sealed class DubUrlExecutionEngine : IExecutionEngine
{
    private readonly DubUrlClient client;
    private readonly DbCommand command;

    public DubUrlExecutionEngine(DubUrlClient client, DbCommand command)
    {
        this.client = client;
        this.command = command;
        var connection = command.Connection;
        command.Connection = null;
        connection?.Dispose();
    }

    public DataSet Execute() => Run(command =>
    {
        using var adapter = new QueryDataAdapter { SelectCommand = command };
        var result = new DataSet();
        adapter.Fill(result);
        return result;
    });

    public object? ExecuteScalar() => Run(command => command.ExecuteScalar());

    public IEnumerable<T> ExecuteList<T>() => Run(command =>
    {
        using var reader = command.ExecuteReader();
        var results = new List<T>();
        while (reader.Read())
        {
            var value = reader.GetValue(0);
            results.Add(value is T result ? result : (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture));
        }
        return results;
    });

    private T Run<T>(Func<DbCommand, T> execute)
    {
        using var connection = (DbConnection)client.CreateConnection();
        connection.Open();

        command.Connection = connection;
        try
        {
            return execute(command);
        }
        finally
        {
            command.Connection = null;
        }
    }

    private sealed class QueryDataAdapter : DbDataAdapter;
}
