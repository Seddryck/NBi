using System;
using System.Collections.Generic;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory: EngineFactory<IExecutionEngine>
    {

        public ExecutionEngineFactory()
        {
            RegisterEngines(new[] {
                typeof(AdomdExecutionEngine),
                typeof(OdbcExecutionEngine),
                typeof(OleDbExecutionEngine),
                typeof(SqlExecutionEngine) }
            );
        }

    }
}
