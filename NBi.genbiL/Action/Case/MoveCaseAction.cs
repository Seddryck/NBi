using NBi.GenbiL.Stateful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Case;

public class MoveCaseAction : ISingleCaseAction
{
    public string VariableName { get; set; }

    /// <summary>
    /// Int.Min = First
    /// -1 = left
    /// +1 = right
    /// Int.Max = Last
    /// </summary>
    public int Ordinal { get; set; }
    public MoveCaseAction(string variableName, int ordinal)
    {
        VariableName = variableName;
        Ordinal = ordinal;
    }

    public void Execute(GenerationState state) => Execute(state.CaseCollection.CurrentScope);

    public void Execute(CaseSet testCases)
    {
        var currentPosition = testCases.Variables.ToList().IndexOf(VariableName);

        //Calculate new position
        var newPosition = (Ordinal != int.MinValue && Ordinal != int.MaxValue)
             ? currentPosition + Ordinal
             : (Ordinal == int.MinValue)
                ? 0 : testCases.Variables.Count() - 1;

        if (!testCases.Variables.Contains(VariableName))
            throw new ArgumentOutOfRangeException("variableName");

        testCases.Content.Columns[currentPosition].SetOrdinal(newPosition);
        testCases.Content.AcceptChanges();
    }

    public string Display
        => $"Moving column '{VariableName}' to the{(Ordinal != int.MinValue && Ordinal != int.MaxValue ? "" : " extreme")} {(Ordinal > 1 ? "right" : "left")}";
}
