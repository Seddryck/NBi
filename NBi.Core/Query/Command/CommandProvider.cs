using NBi.Core.Configuration;
using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility;

namespace NBi.Core.Query.Command
{
    public class CommandProvider
    {
        private readonly IList<ICommandFactory> factories = [];
        private readonly Type[] classics =
            [
                typeof(DubUrlCommandFactory),
                typeof(AdomdCommandFactory),
                typeof(OdbcCommandFactory),
                typeof(OleDbCommandFactory),
                typeof(SqlCommandFactory)
            ];

        public CommandProvider()
        {
            RegisterFactories(classics);
        }

        public CommandProvider(IExtensionsConfiguration config)
        {
            var extensions = config?.Extensions?.Where(x => typeof(ICommandFactory).IsAssignableFrom(x.Key) && !x.Key.IsAbstract)?.Select(x => x.Key) ?? [];
            RegisterFactories(classics.Union(extensions).ToArray());
        }

        protected internal void RegisterFactories(Type[] types)
        {
            foreach (var type in types)
            {
                var ctor = type.GetConstructor([]) ?? throw new NBiException($"Can't load an extension. Can't find a constructor without parameters for the type '{type.Name}'");
                var factory = (ICommandFactory)ctor.Invoke([]);
                if (factories.SingleOrDefault(x => x.GetType() == factory.GetType()) != null)
                    throw new ArgumentException($"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.", nameof(types));
                factories.Add(factory);
            }
        }

        public ICommand Instantiate(IClient session, IQuery query)
        {
            foreach (var factory in factories)
                if (factory.CanHandle(session))
                    return factory.Instantiate(session, query, new StringTemplateEngine());
            throw new ArgumentException($"NBi is not able to identify the command factory for a connection supporting the underlying type: {session.UnderlyingSessionType.Name}", nameof(session));
        }
    }
}
