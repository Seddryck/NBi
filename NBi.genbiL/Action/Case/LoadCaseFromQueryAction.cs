using System;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromQueryAction : ISingleCaseAction
    {
        public string Query { get; set; }
        public string ConnectionString { get; set; }

        protected LoadCaseFromQueryAction() { }

        public LoadCaseFromQueryAction(string query, string connectionString)
        {
            Query = query;
            ConnectionString = connectionString;
        }

        public void Execute(GenerationState state) => Execute(state.TestCaseCollection.CurrentScope);

        public virtual void Execute(TestCases testCases)
        {
            var queryEngineFactory = new ExecutionEngineFactory();
            var queryEngine = queryEngineFactory.Instantiate(new Query(Query, ConnectionString));
            var ds = queryEngine.Execute();
            testCases.Content = ds.Tables[0];
        }

        public string Display => $"Loading TestCases from query '{Query}'";
    }
}
