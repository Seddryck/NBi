using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    public class SessionFactory
    {
        private readonly IList<ISessionFactory> factories = new List<ISessionFactory>();

        public SessionFactory()
        {
            RegisterFactories();
        }

        protected void RegisterFactories()
        {
            factories.Add(new AdomdSessionFactory());
            factories.Add(new OdbcSessionFactory());
            factories.Add(new OleDbSessionFactory());
            factories.Add(new SqlSessionFactory());
            factories.Add(new PowerBiDesktopSessionFactory());
        }

        public void AddFactory(ISessionFactory factory)
        {
            if (factories.SingleOrDefault(x => x.GetType()== factory.GetType()) != null)
                throw new ArgumentException(nameof(factory), $"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.");
            factories.Add(factory);
        }

        public ISession Instantiate(string connectionString)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(connectionString))
                    return factory.Instantiate(connectionString);
            throw new ArgumentException(nameof(connectionString), $"NBi is not able to identify the type of the connection string: {connectionString}");
        }
    }
}
