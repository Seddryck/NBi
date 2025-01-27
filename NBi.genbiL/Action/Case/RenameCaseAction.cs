using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class RenameCaseAction : ISingleCaseAction
{
    public string OldVariableName { get; set; }
    public string NewVariableName { get; set; }
    public RenameCaseAction(string oldVariableName, string newVariableName)
    {
        OldVariableName = oldVariableName;
        NewVariableName = newVariableName;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        var index = testCases.Variables.ToList().FindIndex(v => v == OldVariableName);
        testCases.Content.Columns[index].ColumnName = NewVariableName;
    }

    public string Display
    {
        get
        {
            return string.Format("Renaming column '{0}' into '{1}'", OldVariableName, NewVariableName);
        }
    }
}
