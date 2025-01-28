using NBi.GenbiL.Parser.Valuable;
using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class SubstituteCaseAction : ISingleCaseAction
{
    public string ColumnName { get; private set; }
    public IValuable OldText { get; private set; }
    public IValuable NewText { get; private set; }
    public SubstituteCaseAction(string columnName, IValuable oldText, IValuable newText)
    {
        ColumnName = columnName;
        OldText = oldText;
        NewText = newText;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        if (!testCases.Variables.Contains(ColumnName))
            throw new ArgumentOutOfRangeException(String.Format("No column named '{0}' has been found.",ColumnName));

        var index = testCases.Variables.ToList().FindIndex(v => v == ColumnName);

        foreach (DataRow row in testCases.Content.Rows)
        {
            if ((string)row[ColumnName] != "(none)")
            {
                if (NewText.GetValue(row) != "(none)" && OldText.GetValue(row) != "(none)")
                    row[ColumnName] = ((string)row[ColumnName]).Replace(OldText.GetValue(row), NewText.GetValue(row));
                else
                    row[ColumnName] = "(none)";
            }
        }
    }

    public string Display
    {
        get
        {
            return string.Format("Substituing occurences of {1} from column '{0}' with {2}", ColumnName, OldText.Display, NewText.Display);
        }
    }
}
