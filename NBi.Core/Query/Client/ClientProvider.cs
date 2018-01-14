using NBi.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Client
{
    public class ClientProvider
    {
        private readonly IList<IClientFactory> factories = new List<IClientFactory>();
        private Type[] classics = new[]
            {
                typeof(AdomdClientFactory),
                typeof(OdbcClientFactory),
                typeof(SqlClientFactory),
                typeof(PowerBiDesktopClientFactory),
                typeof(OleDbClientFactory), //It's important to keep this one as the last one because it will handle all the connectionStrings with a provider
            };

        public ClientProvider()
        {
            RegisterFactories(classics);
        }

        public ClientProvider(IExtensionsConfiguration config)
        {
            var extensions = config?.Extensions?.Where(x => typeof(IClientFactory).IsAssignableFrom(x)) ?? new Type[0];
            RegisterFactories(classics.Union(extensions).ToArray());
        }

        protected internal void RegisterFactories(Type[] types)
        {
            foreach (var type in types)
            {
                var ctor = type.GetConstructor(new Type[] { });
                var factory = (IClientFactory)ctor.Invoke(new object[] { });
                if (factories.SingleOrDefault(x => x.GetType() == factory.GetType()) != null)
                    throw new ArgumentException($"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.", nameof(types));
                factories.Add(factory);
            }
        }

        public IClient Instantiate(string connectionString)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(connectionString))
                    return factory.Instantiate(connectionString);
            throw new ArgumentException($"NBi is not able to identify the type of the connection string: {connectionString}", nameof(connectionString));
        }
    }
}
