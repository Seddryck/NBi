using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Graph.Query.Session
{
    class GremlinSession : NBi.Core.Query.Session.ISession
    {
        public const string EndpointToken = "endpoint";
        public const string AuthKeyToken = "authkey";
        public const string DatabaseToken = "database";
        public const string GraphToken = "graph";

        private readonly Uri endpoint;

        protected string AuthKey { get => (string)ConnectionStringTokens[AuthKeyToken]; }
        protected string Endpoint { get => (string)ConnectionStringTokens[EndpointToken]; }
        protected string DatabaseId { get => (string)ConnectionStringTokens[DatabaseToken]; }
        protected string GraphId { get => (string)ConnectionStringTokens[GraphToken]; }
        protected DbConnectionStringBuilder ConnectionStringTokens => new DbConnectionStringBuilder() { ConnectionString = ConnectionString };

        public string ConnectionString { get; }
        public Type UnderlyingSessionType => typeof(CosmosDbSession);
        public object CreateNew() => CreateCosmosDbSession();

        public CosmosDbSession CreateCosmosDbSession()
        {
            return new CosmosDbSession(endpoint, AuthKey, DatabaseId, GraphId);
        }

        internal GremlinSession(string subdomain, string api, string authKey, string databaseId, string graphId)
            : this("https", subdomain, api, 443, authKey, databaseId, graphId)
        { }

        internal GremlinSession(string protocol, string subdomain, string api, int port, string authKey, string databaseId, string graphId)
            : this(protocol, subdomain, api, "azure.com", port, authKey, databaseId, graphId)
        { }

        internal GremlinSession(string protocol, string subdomain, string api, string domain, int port, string authKey, string databaseId, string graphId)
            : this(new UriBuilder(protocol, $"{subdomain}.{api}.{domain}", port).Uri, authKey, databaseId, graphId)
        { }

        internal GremlinSession(Uri endpoint, string authkey, string databaseId, string graphId)
        {
            this.endpoint = endpoint;

            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                { EndpointToken, endpoint.ToString() },
                { AuthKeyToken, authkey },
                { DatabaseToken, databaseId },
                { GraphToken, graphId }
            };
            ConnectionString = connectionStringBuilder.ToString();
        }
    }
}
