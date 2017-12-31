using NBi.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    public class SessionProvider
    {
        private readonly IList<ISessionFactory> factories = new List<ISessionFactory>();
        private Type[] classics = new[]
            {
                typeof(AdomdSessionFactory),
                typeof(OdbcSessionFactory),
                typeof(SqlSessionFactory),
                typeof(PowerBiDesktopSessionFactory),
                typeof(OleDbSessionFactory), //It's important to keep this one as the last one because it will handle all the connectionStrings with a provider
            };

        public SessionProvider()
        {
            RegisterFactories(classics);
        }

        public SessionProvider(IExtensionsConfiguration config)
        {
            var extensions = config?.Extensions?.Where(x => typeof(ISessionFactory).IsAssignableFrom(x)) ?? new Type[0];
            RegisterFactories(classics.Union(extensions).ToArray());
        }

        protected internal void RegisterFactories(Type[] types)
        {
            foreach (var type in types)
            {
                var ctor = type.GetConstructor(new Type[] { });
                var factory = (ISessionFactory)ctor.Invoke(new object[] { });
                if (factories.SingleOrDefault(x => x.GetType() == factory.GetType()) != null)
                    throw new ArgumentException($"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.", nameof(types));
                factories.Add(factory);
            }
        }

        public ISession Instantiate(string connectionString)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(connectionString))
                    return factory.Instantiate(connectionString);
            throw new ArgumentException($"NBi is not able to identify the type of the connection string: {connectionString}", nameof(connectionString));
        }
    }
}
