using NBi.Core.Configuration;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory: EngineFactory<IExecutionEngine>
    {
        private Type[] classics =
        [
            typeof(AdomdExecutionEngine),
            typeof(OdbcExecutionEngine),
            typeof(OleDbExecutionEngine),
            typeof(SqlExecutionEngine)
        ];

        public ExecutionEngineFactory() 
            : base()
        {
            RegisterEngines(classics);
        }

        public ExecutionEngineFactory(ClientProvider clientProvider, CommandProvider commandProvider)
            : base(clientProvider, commandProvider)
        {
            RegisterEngines(classics);
        }

        public ExecutionEngineFactory(ClientProvider clientProvider, CommandProvider commandProvider, IExtensionsConfiguration config)
            : base(clientProvider, commandProvider)
        {
            var extensions = config?.Extensions?.Where(x => typeof(IExecutionEngine).IsAssignableFrom(x.Key))?.Select(x => x.Key) ?? [];
            RegisterEngines(classics.Union(extensions).ToArray());
        }

        internal int ExtensionCount { get => engines.Count - classics.Length; }
    }
}
