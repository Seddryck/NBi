using NBi.Core.Query.Session;
using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Session
{
    public class BoltSessionFactory : ISessionFactory
    {
        public bool CanHandle(string connectionString)
        {
            return connectionString.ToLowerInvariant().StartsWith("bolt://");
        }

        public NBi.Core.Query.Session.ISession Instantiate(string connectionString)
        {
            var tokens = connectionString.Split(new[] { ':', '/', '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Count()!=5 || tokens[0].ToLowerInvariant() != "bolt")
                throw new ArgumentException();

            var server = tokens[3];
            var port = tokens[4];
            var username = tokens[1];
            var password = tokens[2];

            return new BoltSession(server, port, username, password);
        }
    }
}
