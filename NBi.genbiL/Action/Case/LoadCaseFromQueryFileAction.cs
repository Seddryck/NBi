using System;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case;

public class LoadCaseFromQueryFileAction : LoadCaseFromQueryAction
{
    public string Filename { get; set; }

    public LoadCaseFromQueryFileAction(string filename, string connectionString)
    {
        Filename = filename;
        ConnectionString = connectionString;
    }

    public override void Execute(CaseSet testCases)
    {
        Query = System.IO.File.ReadAllText(Filename);
        base.Execute(testCases);
    }

    public new string Display => $"Loading TestCases from query written in '{Filename}'";

}
