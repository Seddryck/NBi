using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    class DbSession : ISession
    {
        private readonly DbProviderFactory factory;

        public string ConnectionString { get; }

        public Type UnderlyingSessionType { get; }

        public DbSession(DbProviderFactory factory, Type underlyingConnectionType, string connectionString)
        {
            UnderlyingSessionType = underlyingConnectionType;
            this.factory = factory;
            ConnectionString = connectionString;
        }

        public object CreateNew() => CreateConnection();

        public IDbConnection CreateConnection()
        {
            var dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = ConnectionString;
            return dbConnection;
        }

    }
}
