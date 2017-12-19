using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    public class ConnectionFactory
    {
        private readonly IList<IConnectionFactory> factories = new List<IConnectionFactory>();

        public ConnectionFactory()
        {
            RegisterFactories();
        }

        protected void RegisterFactories()
        {
            factories.Add(new OlapConnectionFactory());
            factories.Add(new OdbcConnectionFactory());
            factories.Add(new OleDbConnectionFactory());
            factories.Add(new SqlConnectionFactory());
            factories.Add(new PowerBiDesktopConnectionFactory());
        }

        public void AddFactory(IConnectionFactory factory)
        {
            if (factories.SingleOrDefault(x => x.GetType()== factory.GetType()) != null)
                throw new ArgumentException(nameof(factory), $"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.");
            factories.Add(factory);
        }

        public IConnection Instantiate(string connectionString)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(connectionString))
                    return factory.Instantiate(connectionString);
            throw new ArgumentException(nameof(connectionString), $"NBi is not able to identify the type of the connection string: {connectionString}");
        }
    }
}
