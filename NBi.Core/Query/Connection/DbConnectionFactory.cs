using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    class DbConnectionFactory : IConnectionFactory
    {
        private readonly IDictionary<string, string> providers;

        public DbConnectionFactory()
        {
            providers = new Dictionary<string, string>();
        }

        public DbConnectionFactory(IDictionary<string, string> providers)
        {
            this.providers = providers;
        }

        public bool CanHandle(string connectionString)
        {
            return ParseConnectionString(connectionString) != null;
        }

        public IConnection Instantiate(string connectionString)
        {
            var factory = ParseConnectionString(connectionString);
            if (factory == null)
                throw new ArgumentException();

            return new DbConnection(factory, connectionString);
        }

        private DbProviderFactory ParseConnectionString(string connectionString)
        {
            var csb = GetConnectionStringBuilder(connectionString);
            if (csb == null)
                return null;

            var providerName = ExtractProviderName(csb, connectionString);
            if (!string.IsNullOrEmpty(providerName))
                providerName = TranslateProviderName(providerName);
            else
            {
                providerName = ExtractDriverToken(csb, connectionString);
                if (string.IsNullOrEmpty(providerName))
                    providerName = ValidateNative(connectionString);
            }

            if (string.IsNullOrEmpty(providerName))
                return null;

            var factory = GetDbProviderFactory(providerName);
            return factory;
        }

        private DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString)
        {
            try { return new DbConnectionStringBuilder() { ConnectionString = connectionString }; }
            catch (Exception) { return null; }
        }

        private string ExtractProviderName(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
        {
            if (connectionStringBuilder.ContainsKey("Provider"))
                return (connectionStringBuilder["Provider"].ToString());
            return string.Empty;
        }

        private string ExtractDriverToken(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
        {
            if (connectionStringBuilder.ContainsKey("Driver"))
                return ("System.Data.Odbc");

            return string.Empty;
        }

        private string ValidateNative(string connectionString)
        {
            try
            {
                var csb = new SqlConnectionStringBuilder() { ConnectionString = connectionString };
                if (!string.IsNullOrEmpty(csb.DataSource) && !string.IsNullOrEmpty(csb.InitialCatalog))
                    return "System.Data.SqlClient";
            }
            catch (Exception)
            { }
            
            return string.Empty;
        }

        private string TranslateProviderName(string providerName)
        {
            if (providerName.ToLowerInvariant().StartsWith("msolap")) return "Microsoft.AnalysisServices.AdomdClient";
            if (providerName.ToLowerInvariant().StartsWith("sqlncli")) return "System.Data.OleDb"; //Indeed OleDb it's not a mistake! SQL Server Native Client 
            if (providerName.ToLowerInvariant().StartsWith("oledb")) return "System.Data.OleDb";
            if (providerName.ToLowerInvariant().StartsWith("sqloledb")) return "System.Data.OleDb"; // SQL Server OLE DB driver 

            foreach (var provider in providers)
                if (provider.Key.ToLowerInvariant() == providerName.ToLowerInvariant())
                    return provider.Value;
            return null;
        }

        private DbProviderFactory GetDbProviderFactory(string providerName)
        {
            var providers = new List<string>();
            foreach (DataRowView item in DbProviderFactories.GetFactoryClasses().DefaultView)
                providers.Add((string)item[2]);

            var invariantNames = providers.FindAll(p => p.ToLowerInvariant() == providerName.ToLowerInvariant());

            if (invariantNames.Count == 1)
                return DbProviderFactories.GetFactory(invariantNames[0]);
            else if (invariantNames.Count > 1)
                throw new ArgumentException(string.Format("More than one Provider can be returned based on providerName given: '{0}'", providerName));

            return null;
        }

    }
}

