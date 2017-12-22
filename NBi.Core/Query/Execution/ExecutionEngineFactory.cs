using NBi.Core.Query.Command;
using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;

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

        protected internal ExecutionEngineFactory(SessionFactory sessionFactory, CommandFactory commandFactory)
            : base(sessionFactory, commandFactory)
        {
            RegisterEngines(classics);
        }
    }
}
