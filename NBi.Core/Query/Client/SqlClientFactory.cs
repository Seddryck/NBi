using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using System.Data;

namespace NBi.Core.Query.Client
{
    class SqlClientFactory : DbClientFactory
    {
        protected override DbProviderFactory? ParseConnectionString(string connectionString)
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

        protected override DbConnectionStringBuilder? GetConnectionStringBuilder(string connectionString)
        {
            try { return new SqlConnectionStringBuilder() { ConnectionString = connectionString }; }
            catch (Exception) { return null; }
        }

        protected override DbProviderFactory? GetDbProviderFactory(string providerName)
        {
            var providers = new List<string>();
            foreach (DataRowView item in DbProviderFactories.GetFactoryClasses().DefaultView)
                providers.Add((string)item[2]);

            if (!providers.Any(x => x == providerName))
                DbProviderFactories.RegisterFactory(providerName, Microsoft.Data.SqlClient.SqlClientFactory.Instance);

            return base.GetDbProviderFactory(providerName);
        }

        protected virtual string ValidateNative(SqlConnectionStringBuilder csb, string connectionString)
        {
            if (!string.IsNullOrEmpty(csb.DataSource) && !string.IsNullOrEmpty(csb.InitialCatalog))
                return "Microsoft.Data.SqlClient";
            return string.Empty;
        }

    }
}
