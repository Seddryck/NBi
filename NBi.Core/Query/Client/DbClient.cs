using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    class DbClient : IClient
    {
        private readonly DbProviderFactory factory;

        public string ConnectionString { get; }

        public Type UnderlyingSessionType { get; }

        public DbClient(DbProviderFactory factory, Type underlyingConnectionType, string connectionString)
        {
            UnderlyingSessionType = underlyingConnectionType;
            this.factory = factory;
            ConnectionString = connectionString;
        }

        public object CreateNew() => CreateConnection();

        public IDbConnection CreateConnection()
        {
            var dbConnection = factory.CreateConnection() ?? throw new ConnectionException(new Exception(), ConnectionString);
            dbConnection.ConnectionString = ConnectionString;
            return dbConnection;
        }

    }
}
