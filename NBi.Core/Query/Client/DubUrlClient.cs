using DubUrl;
using DubUrl.Extensions;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Extensibility.Query;
using System.Data;

namespace NBi.Core.Query.Client;

internal class DubUrlClient(ConnectionUrl url, string connectionString) : IClient
{
    public string ConnectionString { get; } = connectionString;
    internal ConnectionUrl Url { get; } = url;

    public Type UnderlyingSessionType
    {
        get
        {
            using var connection = CreateConnection();
            return connection.GetType();
        }
    }

    public object CreateNew() => CreateConnection();

    public IDbConnection CreateConnection()
    {
        var connection = Url.Connect();
        if (connection is not IDubUrlConnectionWrapper { Connection: AdomdConnection })
            return connection;

        using (connection)
            return new AdomdConnection(connection.ConnectionString);
    }
}
