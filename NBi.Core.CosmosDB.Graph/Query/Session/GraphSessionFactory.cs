using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Session
{
    public class GraphSessionFactory : ISessionFactory
    {
        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(GraphSession.EndpointToken) && connectionStringBuilder.ContainsKey(GraphSession.AuthKeyToken);
        }

        public ISession Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var endpoint = new Uri((string)connectionStringBuilder[GraphSession.EndpointToken]);
            var authkey = (string)connectionStringBuilder[GraphSession.AuthKeyToken];
            var database = (string)connectionStringBuilder[GraphSession.DatabaseToken];
            var graph = (string)connectionStringBuilder[GraphSession.GraphToken];

            return new GraphSession(endpoint, authkey, database, graph);
        }
    }
}
