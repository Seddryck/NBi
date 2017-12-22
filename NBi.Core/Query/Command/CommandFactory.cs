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

        public CommandFactory()
        {
            RegisterFactories();
        }

        protected void RegisterFactories()
        {
            factories.Add(new AdomdCommandFactory());
            factories.Add(new OdbcCommandFactory());
            factories.Add(new OleDbCommandFactory());
            factories.Add(new SqlCommandFactory());
        }

        public void AddFactory(ICommandFactory factory)
        {
            if (factories.SingleOrDefault(x => x.GetType()== factory.GetType()) != null)
                throw new ArgumentException(nameof(factory), $"You can't add twice the same factory. The factory '{factory.GetType().Name}' was already registered.");
            factories.Add(factory);
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
