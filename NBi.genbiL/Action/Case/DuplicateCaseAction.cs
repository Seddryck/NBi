using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

class DuplicateCaseAction : ISingleCaseAction
{
    public string OriginalColumn { get; }
    public IEnumerable<string> NewColumns { get; }

    public DuplicateCaseAction(string originalColumn, IEnumerable<string> newColumns)
    {
        this.OriginalColumn = originalColumn;
        this.NewColumns = newColumns;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        foreach (var newColumn in NewColumns)
        {
            var dataTable = testCases.Content;
            dataTable.Columns.Add(new DataColumn(newColumn, typeof(object)) { AllowDBNull = true, DefaultValue = DBNull.Value });

            foreach (DataRow row in dataTable.Rows)
                if (!row.IsNull(OriginalColumn))
                    row[newColumn] = row[OriginalColumn];
        }
    }

    public string Display => string.Format($"Duplicating column '{OriginalColumn}' as new column{(NewColumns.Count() > 1 ? "s" : string.Empty)} '{string.Join("', '", NewColumns)}'", NewColumns);
}

