using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Command;
using NBi.Core.Query.Session;

namespace NBi.Core.Query.Execution
{
    /// <summary>
    /// Class to retrieve an adequate query engine on base of the connectionString
    /// </summary>
    public class ExecutionEngineFactory
    {
        private readonly IDictionary<string, Type> engines = new Dictionary<string, Type>();

        public ExecutionEngineFactory()
        {
            RegisterEngines();
        }

        private void RegisterEngines()
        {
            var types = new[] { typeof(AdomdExecutionEngine), typeof(OdbcExecutionEngine), typeof(OleDbExecutionEngine), typeof(SqlExecutionEngine) };

            foreach (var t in types)
            {
                var name = t.GetAttributeValue((SupportedCommandTypeAttribute x) => x.Value).FullName;
                engines.Add(name, t);
            }
        }

        public IExecutionEngine Instantiate(IQuery query)
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

        protected IExecutionEngine Instantiate(Type type, ICommand cmd)
        {
            var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { cmd.Implementation.GetType() }, null);
            return ctor.Invoke(new[] { cmd.Implementation }) as IExecutionEngine;
        }

    }
}
