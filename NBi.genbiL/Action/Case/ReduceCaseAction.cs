using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class ReduceCaseAction : ISingleCaseAction
{
    public List<string> ColumnNames { get; set; }

    public ReduceCaseAction(IEnumerable<string> variableNames)
    {
        this.ColumnNames = new List<string>(variableNames);
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {

        foreach (DataRow row in testCases.Content.Rows)
        {
            foreach (var columnName in ColumnNames)
            {
                if (row[columnName] is IEnumerable<string> list)
                    row[columnName] = list.Distinct().ToArray();
            }
        }
    }

    public string Display
    {
        get
        {
            if (ColumnNames.Count == 1)
                return string.Format("Reducing the length of groups for column '{0}'", ColumnNames[0]);
            else
                return string.Format("Reducing the length of groups for columns '{0}'", String.Join("', '", ColumnNames));
        }
    }
}
