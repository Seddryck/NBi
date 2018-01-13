using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Graph.Query.Session
{
    public class GremlinSessionFactory : ISessionFactory
    {
        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(GremlinSession.EndpointToken) && connectionStringBuilder.ContainsKey(GremlinSession.AuthKeyToken);
        }

        public ISession Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var endpoint = new Uri((string)connectionStringBuilder[GremlinSession.EndpointToken]);
            var authkey = (string)connectionStringBuilder[GremlinSession.AuthKeyToken];
            var database = (string)connectionStringBuilder[GremlinSession.DatabaseToken];
            var graph = (string)connectionStringBuilder[GremlinSession.GraphToken];

            return new GremlinSession(endpoint, authkey, database, graph);
        }
    }
}
