using NBi.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    class OleDbConnectionFactory : DbConnectionFactory
    {
        private readonly IReadOnlyDictionary<string, string> providers;

        public OleDbConnectionFactory() 
            : base()
        {
            providers = ConfigurationManager.GetConfiguration().Providers;
        }

        public OleDbConnectionFactory(IDictionary<string, string> providers)
            : base()
        {
            this.providers = new ReadOnlyDictionary<string, string>(providers);
        }

        protected override DbProviderFactory ParseConnectionString(string connectionString)
        {
            var csb = GetConnectionStringBuilder(connectionString);
            if (csb == null)
                return null;

            var providerName = ExtractProviderName(csb, connectionString);
            if (string.IsNullOrEmpty(providerName))
                return null;
            providerName = TranslateProviderName(providerName);
            if (string.IsNullOrEmpty(providerName))
                return null;

            var factory = GetDbProviderFactory(providerName);
            return factory;
        }

        private string ExtractProviderName(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
        {
            if (connectionStringBuilder.ContainsKey("Provider"))
                return (connectionStringBuilder["Provider"].ToString());
            return string.Empty;
        }

        private string TranslateProviderName(string providerName)
        {
            if (providerName.ToLowerInvariant().StartsWith("sqlncli")) return "System.Data.OleDb"; //Indeed OleDb it's not a mistake! SQL Server Native Client 
            if (providerName.ToLowerInvariant().StartsWith("oledb")) return "System.Data.OleDb";
            if (providerName.ToLowerInvariant().StartsWith("sqloledb")) return "System.Data.OleDb"; // SQL Server OLE DB driver 

            foreach (var provider in providers)
                if (provider.Key.ToLowerInvariant() == providerName.ToLowerInvariant())
                    return provider.Value;
            return null;
        }
    }
}
