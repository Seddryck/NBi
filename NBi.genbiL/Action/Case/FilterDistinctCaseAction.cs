using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class FilterDistinctCaseAction: ISingleCaseAction
{

    public FilterDistinctCaseAction()
    { }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        DataTableReader? dataReader = null;

        var content = testCases.Content;
        var distinctRows = content.AsEnumerable().Distinct(DataRowComparer.Default);

        if (distinctRows.Any())
        {
            var distinctTable = distinctRows.CopyToDataTable();
            dataReader = distinctTable.CreateDataReader();
        }
        content.Clear();

        if (dataReader != null)
            content.Load(dataReader, LoadOption.PreserveChanges);
        content.AcceptChanges();
    }

    public virtual string Display { get => $"Filtering distinct cases."; }
}
