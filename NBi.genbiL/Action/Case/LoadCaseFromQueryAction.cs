using System;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public class LoadCaseFromQueryAction : ISingleCaseAction
{
    public string Query { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;

    protected LoadCaseFromQueryAction() { }

    public LoadCaseFromQueryAction(string query, string connectionString)
    {
        Query = query;
        ConnectionString = connectionString;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public virtual void Execute(CaseSet testCases)
    {
        var queryEngineFactory = new ExecutionEngineFactory();
        var queryEngine = queryEngineFactory.Instantiate(new Query(Query, ConnectionString));
        var ds = queryEngine.Execute();
        testCases.Content = ds.Tables[0];
    }

    public string Display => $"Loading TestCases from query '{Query}'";
}
