using System;
using System.Linq;
using NBi.Service;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class LoadCaseFromQueryAction : ICaseAction
    {
        public string Query { get; set; }
        public string ConnectionString { get; set; }

        public LoadCaseFromQueryAction(string query, string connectionString)
        {
            Query = query;
            ConnectionString = connectionString;
        }

        public virtual void Execute(GenerationState state)
        {
            state.TestCaseCollection.Scope.ReadFromQuery(Query, ConnectionString);
        }

        public string Display
        {
            get
            {
                return string.Format("Loading TestCases from query '{0}'"
                    , Query);
            }
        }
       

    }
}
