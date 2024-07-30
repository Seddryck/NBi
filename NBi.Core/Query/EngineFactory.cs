using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query
{
    public abstract class EngineFactory<T>
    {
        protected readonly IDictionary<string, Type> engines = new Dictionary<string, Type>();
        private readonly ClientProvider sessionFactory;
        private readonly CommandProvider commandFactory;

        protected internal EngineFactory()
        {
            this.sessionFactory = new ClientProvider();
            this.commandFactory = new CommandProvider();
        }

        protected internal EngineFactory(ClientProvider sessionFactory, CommandProvider commandFactory)
        {
            this.sessionFactory = sessionFactory;
            this.commandFactory = commandFactory;
        }

        protected internal void RegisterEngines(Type[] types)
        {
            var invalidTypes = new List<string>();
            foreach (var type in types)
            {
                var underlyingType = type.GetAttributeValue((SupportedCommandTypeAttribute x) => x?.Value);
                if (underlyingType == null)
                    invalidTypes.Add(type.FullName ?? "type without name");
                else
                    engines.Add(underlyingType.FullName ?? "type without name", type);
            }
            if (invalidTypes.Count > 0)
                throw new ArgumentException($"Unable to find the attribute SupportedCommandType for the type{(invalidTypes.Count>1 ? "s" : string.Empty)}: '{string.Join(@"', '", invalidTypes)}'.");
        }

        public virtual T Instantiate(IQuery query)
        {
            var session = sessionFactory.Instantiate(query.ConnectionString);
            var cmd = commandFactory.Instantiate(session, query);

            var key = cmd.Implementation.GetType().FullName ?? throw new NullReferenceException();
            if (engines.ContainsKey(key))
                return Instantiate(engines[key], cmd);
            throw new ArgumentException();
        }

        protected T Instantiate(Type type, ICommand cmd)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var types = new[] { cmd.Client.GetType(), cmd.Implementation.GetType() };
            var ctor = type.GetConstructor(flags, null, types, null);
            return ctor == null
                ? throw new ArgumentException($"Unable to find a constructor for the type '{type.FullName}' exposing the following parameters: '{string.Join("', '", types.Select(x => x.FullName))}'")
                : (T)ctor.Invoke([cmd.Client, cmd.Implementation]);
        }
    }
}
