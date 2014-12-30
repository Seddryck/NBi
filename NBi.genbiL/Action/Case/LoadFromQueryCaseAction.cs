using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;
using System.Data;
using NBi.Core.Query;

namespace NBi.GenbiL.Action.Case
{
    public class LoadFromQueryCaseAction : ICaseAction
    {
        public string Query { get; set; }
        public string ConnectionString { get; set; }

        public LoadFromQueryCaseAction(string query, string connectionString)
        {
            Query = query;
            ConnectionString = connectionString;
        }

        public void Execute(GenerationState state)
        {
            var queryEngineFactory = new QueryEngineFactory();
            var queryEngine = queryEngineFactory.GetExecutor(GetQuery(), ConnectionString);
            var ds = queryEngine.Execute();

            var dr = ds.Tables[0].CreateDataReader();
            state.TestCaseSetCollection.Scope.Content.Load(dr, LoadOption.PreserveChanges);
            state.TestCaseSetCollection.Scope.Content.AcceptChanges();
        }

        protected virtual string GetQuery()
        {
            return Query;
        }

        public string Display
        {
            get
            {
                return string.Format("Loading set of test-cases from query '{0}'"
                    , Query);
            }
        }
       

    }
}
