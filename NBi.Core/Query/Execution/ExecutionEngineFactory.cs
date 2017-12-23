using NBi.Core.Configuration;
using NBi.Core.Query.Command;
using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory: EngineFactory<IExecutionEngine>
    {
        private Type[] classics = new[] 
        {
            typeof(AdomdExecutionEngine),
            typeof(OdbcExecutionEngine),
            typeof(OleDbExecutionEngine),
            typeof(SqlExecutionEngine)
        };

        public ExecutionEngineFactory() 
            : base()
        {
            RegisterEngines(classics);
        }

        public ExecutionEngineFactory(SessionFactory sessionFactory, CommandFactory commandFactory)
            : base(sessionFactory, commandFactory)
        {
            RegisterEngines(classics);
        }

        public ExecutionEngineFactory(SessionFactory sessionFactory, CommandFactory commandFactory, IExtensionsConfiguration config)
            : base(sessionFactory, commandFactory)
        {
            var extensions = config?.Extensions?.Where(x => typeof(IExecutionEngine).IsAssignableFrom(x)) ?? new Type[0];
            RegisterEngines(classics.Union(extensions).ToArray());
        }
    }
}
