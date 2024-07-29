using DubUrl;
using DubUrl.Mapping;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Client
{
    internal class DubUrlClient : IClient
    {
        public string ConnectionString { get; }
        private SchemeMapperBuilder SchemeMapperBuilder { get; }

        public Type UnderlyingSessionType => throw new NotImplementedException();

        public DubUrlClient(string connectionString, SchemeMapperBuilder schemeMapperBuilder)
            => (SchemeMapperBuilder, ConnectionString) = (schemeMapperBuilder, connectionString);

        public object CreateNew() => CreateConnection();

        public IDbConnection CreateConnection()
            => new ConnectionUrl(ConnectionString, SchemeMapperBuilder).Connect();
    }
}
