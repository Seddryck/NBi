using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    public class GraphSessionFactory : IClientFactory
    {
        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(GraphClient.EndpointToken) && connectionStringBuilder.ContainsKey(GraphClient.AuthKeyToken);
        }

        public IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var endpoint = new Uri((string)connectionStringBuilder[GraphClient.EndpointToken]);
            var authkey = (string)connectionStringBuilder[GraphClient.AuthKeyToken];
            var database = (string)connectionStringBuilder[GraphClient.DatabaseToken];
            var graph = (string)connectionStringBuilder[GraphClient.GraphToken];

            return new GraphClient(endpoint, authkey, database, graph);
        }
    }
}
