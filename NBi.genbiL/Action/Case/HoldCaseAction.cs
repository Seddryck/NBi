using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class HoldCaseAction : ISingleCaseAction
{
    private List<string> VariableNames { get; set; }

    public IReadOnlyList<string> Variables
    {
        get
        {
            return VariableNames.AsReadOnly();
        }
    }
    public HoldCaseAction(string variableName)
    {
        VariableNames = new List<string>() { variableName };
    }

    public HoldCaseAction(IEnumerable<string> variableNames)
    {
        this.VariableNames = new List<string>(variableNames);
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        var variablesDeleted = testCases.Variables.Except(VariableNames).ToList();

        foreach (var variable in variablesDeleted)
            testCases.Content.Columns.Remove(variable);
    }

    public string Display
    {
        get
        {
            if (VariableNames.Count == 1)
                return string.Format("Holding column '{0}'", VariableNames[0]);
            else
                return string.Format("Holding columns '{0}'", String.Join("', '", VariableNames));
        }
    }
}
