using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core
{
    public class ConnectionFactory
    {

        public static IDbConnection Get(string connectionString)
        {
            var csb = new DbConnectionStringBuilder();
            csb.ConnectionString = connectionString;

            string providerName= 
                csb.ContainsKey("Provider") ? providerName = InterpretProviderName(csb["Provider"].ToString()) : providerName = "SqlClient";

            if (string.IsNullOrEmpty(providerName))
                throw new ArgumentException(string.Format("No provider found forconnectionString '{0}'", connectionString));
                
            return Get(providerName, connectionString);           
        }

        public static string InterpretProviderName(string provider)
        {
            switch (provider.ToLowerInvariant())
            {
                case "msolap": return "OleDb";
                case "sqlncli": return "OleDb";
                default:
                    break;
            }

            if (provider.StartsWith("Driver={"))
                return "Odbc";

            return null;
        }

        public static IDbConnection Get(string providerName, string connectionString)
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

        protected static string TranslateProviderName(string providerName)
        {
            var providers = new List<string>();
            foreach (DataRowView item in DbProviderFactories.GetFactoryClasses().DefaultView)
                providers.Add((string)item[2]);

            var invariantNames = providers.FindAll(p => p.ToLowerInvariant().Contains(providerName.ToLowerInvariant()));

            if (invariantNames.Count==1)
                return invariantNames[0];
            else if (invariantNames.Count>1)
                throw new ArgumentException(string.Format("More than one Provider can be returned based on providerName given: '{0}'", providerName));

            return null;
        }

        protected static IDbConnection GetSpecific(string providerName)
        {
            switch (providerName.ToLowerInvariant())
            {
                case "adomd": return new AdomdConnection();
                default:
                    break;
            }
            return null;
        }
    }
}
