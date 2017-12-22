using NBi.Core.Query.Command;
using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query
{
    public abstract class EngineFactory<T>
    {
        private readonly IDictionary<string, Type> engines = new Dictionary<string, Type>();

        protected void RegisterEngines(Type[] types)
        {
            foreach (var t in types)
            {
                var name = t.GetAttributeValue((SupportedCommandTypeAttribute x) => x.Value).FullName;
                engines.Add(name, t);
            }
        }

        public T Instantiate(IQuery query)
        {
            var sessionFactory = new SessionFactory();
            var session = sessionFactory.Instantiate(query.ConnectionString);

            var commandFactory = new CommandFactory();
            var cmd = commandFactory.Instantiate(session, query);

            var key = cmd.Implementation.GetType().FullName;
            if (engines.ContainsKey(key))
                return Instantiate(engines[key], cmd);
            throw new ArgumentException();
        }

        protected T Instantiate(Type type, ICommand cmd)
        {
            var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { cmd.Session.GetType(), cmd.Implementation.GetType() }, null);
            return (T)ctor.Invoke(new[] { cmd.Session, cmd.Implementation });
        }
    }
}
