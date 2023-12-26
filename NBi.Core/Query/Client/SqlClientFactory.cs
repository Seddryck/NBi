using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    class SqlClientFactory : DbClientFactory
    {
        protected override DbProviderFactory ParseConnectionString(string connectionString)
        {
            var csb = GetConnectionStringBuilder(connectionString) as SqlConnectionStringBuilder;
            if (csb == null)
                return null;

            var providerName = ValidateNative(csb, connectionString);
            if (string.IsNullOrEmpty(providerName))
                return null;

            var factory = GetDbProviderFactory(providerName);
            return factory;
        }

        protected override IClient Instantiate(DbProviderFactory factory, string connectionString)
            => new DbClient(factory, typeof(SqlConnection), connectionString);

        protected override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString)
        {
            try { return new SqlConnectionStringBuilder() { ConnectionString = connectionString }; }
            catch (Exception) { return null; }
        }

        private string ValidateNative(SqlConnectionStringBuilder csb, string connectionString)
        {
            if (!string.IsNullOrEmpty(csb.DataSource) && !string.IsNullOrEmpty(csb.InitialCatalog))
                return "System.Microsoft.SqlClient";
            return string.Empty;
        }

    }
}
