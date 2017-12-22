using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    public class CommandFactory
    {
        private readonly IList<ICommandFactory> factories = new List<ICommandFactory>();
        private Type[] classics = new[]
            {
                typeof(AdomdCommandFactory),
                typeof(OdbcCommandFactory),
                typeof(OleDbCommandFactory),
                typeof(SqlCommandFactory)
            };

        public CommandFactory()
        {
            var extensions = Configuration.ConfigurationManager.GetConfiguration().Extensions.Where(x => typeof(ICommandFactory).IsAssignableFrom(x));
            RegisterFactories(classics.Union(extensions).ToArray());
        }

        protected internal void RegisterFactories(Type[] types)
        {
            foreach (var type in types)
            {
                var ctor = type.GetConstructor(new Type[] { });
                var factory = (ICommandFactory)ctor.Invoke(new object[] { });
                if (factories.SingleOrDefault(x => x.GetType() == factory.GetType()) != null)
                    throw new ArgumentException($"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.", nameof(types));
                factories.Add(factory);
            }
        }

        public ICommand Instantiate(ISession session, IQuery query)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(session))
                    return factory.Instantiate(session, query);
            throw new ArgumentException(nameof(session), $"NBi is not able to identify the command factory for a connection supporting the underlying type: {session.UnderlyingSessionType.Name}");
        }
    }
}
