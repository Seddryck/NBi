using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client;

class AdomdClient : IClient
{
    public string ConnectionString { get; }

    public Type UnderlyingSessionType => typeof(AdomdConnection);

    public AdomdClient(string connectionString)
        => ConnectionString = connectionString;

    public object CreateNew() => CreateConnection();

    public AdomdConnection CreateConnection()
        => new AdomdConnection(ConnectionString);
}
