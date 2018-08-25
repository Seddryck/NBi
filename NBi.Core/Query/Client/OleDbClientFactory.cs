using NBi.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    class OleDbClientFactory : DbClientFactory
    {
        public OleDbClientFactory() 
            : base()
        { }

        protected override IClient Instantiate(DbProviderFactory factory, string connectionString)
            => new DbClient(factory, typeof(OleDbConnection), connectionString);

        protected override DbProviderFactory ParseConnectionString(string connectionString)
        {
            var csb = GetConnectionStringBuilder(connectionString);
            if (csb == null)
                return null;

            var providerName = ExtractProviderName(csb, connectionString);
            if (string.IsNullOrEmpty(providerName))
                return null;
            
            var factory = GetDbProviderFactory("System.Data.OleDb");
            return factory;
        }

        private string ExtractProviderName(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
        {
            if (connectionStringBuilder.ContainsKey("Provider"))
                return (connectionStringBuilder["Provider"].ToString());
            return string.Empty;
        }
    }
}
