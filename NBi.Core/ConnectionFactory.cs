using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Configuration;
using NBi.Core.PowerBiDesktop;

namespace NBi.Core
{
    /// <summary>
    /// The ConnectionFactory is used to build an IDbConnection compatible with the connectionString provided.
    /// The first idea is to check the provider information given by the connection-string
    /// If this provider matches with a predifined value or with a configured-value then we assign the class
    /// If not we take a look to the keyword 'driver' and guess t's odbc
    /// If nothing is found we'll assume it's SqlClient
    /// To build the connection of the class, we first rely on the DbProviderFactory 
    /// If we can't build the connection with the help of this DbProviderFactory then we've hardcoded a way to build an Adomd value
    /// </summary>
    public class ConnectionFactory
    {
        private readonly IReadOnlyDictionary<string, string> customProviders;

        public ConnectionFactory()
        {
            customProviders = ConfigurationManager.GetConfiguration().Providers;
        }

        public ConnectionFactory(IReadOnlyDictionary<string, string> customProviders)
        {
            this.customProviders = customProviders;
        }
 
        public IDbConnection Get(string connectionString)
        {
            var csb = new DbConnectionStringBuilder();
            csb.ConnectionString = connectionString;

            string providerName = string.Empty;
            if (csb.ContainsKey("pbix"))
            {
                providerName = "Microsoft.AnalysisServices.AdomdClient";
                var connectionStringBuilder = GetPowerBiDesktopConnectionStringBuilder();
                connectionStringBuilder.Build(csb["pbix"].ToString());
                connectionString = connectionStringBuilder.GetConnectionString();
            }

            if (csb.ContainsKey("Provider"))
                providerName = InterpretProviderName(csb["Provider"].ToString());

            if (string.IsNullOrEmpty(providerName) && csb.ContainsKey("Driver"))
                providerName = "System.Data.Odbc";

            if (string.IsNullOrEmpty(providerName))
                providerName = "System.Data.SqlClient";

            if (string.IsNullOrEmpty(providerName))
                throw new ArgumentException(string.Format("No provider found for connectionString '{0}'", connectionString));
                
            return Get(providerName, connectionString);           
        }

        protected virtual PowerBiDesktopConnectionStringBuilder GetPowerBiDesktopConnectionStringBuilder()
        {
            return new PowerBiDesktopConnectionStringBuilder();
        }

        protected string InterpretProviderName(string provider)
        {
            if (customProviders.ContainsKey(provider))
                return customProviders[provider];

            if (provider.ToLowerInvariant().StartsWith("msolap")) return "Microsoft.AnalysisServices.AdomdClient";
            if (provider.ToLowerInvariant().StartsWith("sqlncli")) return "System.Data.OleDb"; //Indeed OleDb it's not a mistake!
            if (provider.ToLowerInvariant().StartsWith("oledb")) return "System.Data.OleDb";
            
            return null;
        }

        protected internal IDbConnection Get(string providerName, string connectionString)
        {
            var providerInvariantName = TranslateProviderName(providerName);

            IDbConnection conn = null;
            if (string.IsNullOrEmpty(providerInvariantName))
                conn = GetSpecific(providerName);
            else
                conn = DbProviderFactories.GetFactory(providerInvariantName).CreateConnection();

            if(conn == null)
                throw new ArgumentException(string.Format("No provider found for providerName given: '{0}'", providerName));

            conn.ConnectionString = connectionString;
            return conn;
        }

        protected string TranslateProviderName(string providerName)
        {
            var providers = new List<string>();
            foreach (DataRowView item in DbProviderFactories.GetFactoryClasses().DefaultView)
                providers.Add((string)item[2]);

            var invariantNames = providers.FindAll(p => p.ToLowerInvariant()==providerName.ToLowerInvariant());

            if (invariantNames.Count==1)
                return invariantNames[0];
            else if (invariantNames.Count>1)
                throw new ArgumentException(string.Format("More than one Provider can be returned based on providerName given: '{0}'", providerName));

            return null;
        }

        protected IDbConnection GetSpecific(string providerName)
        {
            switch (providerName)
            {
                case "Microsoft.AnalysisServices.AdomdClient": return new AdomdConnection();
                default:
                    break;
            }
            return null;
        }
    }
}
