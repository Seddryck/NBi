using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Neo4j.Query.Client
{
    class BoltClient : NBi.Core.Query.Client.IClient
    {
        private readonly string connectionString;
        private readonly IDriver driver;

        private string protocol { get => "bolt"; }
        public IAuthToken Credentials { get; }

        public string ConnectionString => connectionString;

        public Type UnderlyingSessionType => typeof(ISession);

        public object CreateNew() => CreateSession();

        public ISession CreateSession() => driver.Session();

        public BoltClient(string server, string port, string username, string password)
        {
            connectionString = $@"{protocol}://{username}:{password}@{server}:{port}/";
            Credentials = AuthTokens.Basic(username, password);
            driver = GraphDatabase.Driver($"{protocol}://{server}:{port}", Credentials);
        }
    }
}
