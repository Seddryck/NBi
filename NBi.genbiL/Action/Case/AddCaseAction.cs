using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Action.Case;

public class AddCaseAction : ISingleCaseAction
{
    
    public string VariableName { get; private set; }
    public string DefaultValue { get; private set; }
    public AddCaseAction(string variableName)
        : this(variableName, "(none)")
    {
    }

    public AddCaseAction(string variableName, string defaultValue)
    {
        VariableName = variableName;
        DefaultValue = defaultValue;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        if (testCases.Variables.Contains(VariableName))
            throw new ArgumentException($"Variable '{VariableName}' already existing.");

        var newColumn = new DataColumn(VariableName) { DefaultValue = DefaultValue };
        testCases.Content.Columns.Add(newColumn);
    }

    

    public string Display => $"Adding column '{VariableName}'";
}
