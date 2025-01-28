using System;
using System.Linq;
using NBi.GenbiL.Stateful;
using System.Data;

namespace NBi.GenbiL.Action.Case;

public class LoadCaseFromDatatableAction : ISingleCaseAction
{
    public DataTable DataTable { get; set; }
    public LoadCaseFromDatatableAction(DataTable datatable)
    {
        DataTable = datatable;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        testCases.Content = DataTable.Copy();
        testCases.Content.AcceptChanges();
    }

    public string Display => "Loading TestCases from datatable.";
}