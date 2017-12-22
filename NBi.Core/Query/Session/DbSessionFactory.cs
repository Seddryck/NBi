using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    abstract class DbSessionFactory : ISessionFactory
    {
        public bool CanHandle(string connectionString)
        {
            return ParseConnectionString(connectionString) != null;
        }

        public ISession Instantiate(string connectionString)
        {
            var factory = ParseConnectionString(connectionString);
            if (factory == null)
                throw new ArgumentException();

            return Instantiate(factory, connectionString);
        }

        protected abstract ISession Instantiate(DbProviderFactory factory, string connectionString);

        protected abstract DbProviderFactory ParseConnectionString(string connectionString);

        protected virtual DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString)
        {
            try { return new DbConnectionStringBuilder() { ConnectionString = connectionString }; }
            catch (Exception) { return null; }
        }

        protected DbProviderFactory GetDbProviderFactory(string providerName)
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

