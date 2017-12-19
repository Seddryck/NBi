using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    class DbConnection : IConnection
    {
        private readonly DbProviderFactory factory;

        public string ConnectionString { get; }

        public DbConnection(DbProviderFactory factory, string connectionString)
        {
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
