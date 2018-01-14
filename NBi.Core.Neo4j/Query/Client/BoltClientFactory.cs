using NBi.Core.Query.Client;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Client
{
    public class BoltClientFactory : IClientFactory
    {
        public bool CanHandle(string connectionString)
        {
            return connectionString.ToLowerInvariant().StartsWith("bolt://");
        }

        public NBi.Core.Query.Client.IClient Instantiate(string connectionString)
        {
            var tokens = connectionString.Split(new[] { ':', '/', '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Count()!=5 || tokens[0].ToLowerInvariant() != "bolt")
                throw new ArgumentException();

            var server = tokens[3];
            var port = tokens[4];
            var username = tokens[1];
            var password = tokens[2];

            return new BoltClient(server, port, username, password);
        }
    }
}
