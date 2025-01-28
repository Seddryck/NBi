using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate;

public class RowEvaluationResult
{
    public int RowIndex { get; private set; }
    public Dictionary<string, Object> ValuedVariables { get; private set; }
    public IEnumerable<ExpressionEvaluationResult> Results { get; private set; }

    public RowEvaluationResult(int rowIndex, Dictionary<string, Object> valuedVariables, IEnumerable<ExpressionEvaluationResult> results)
    {
        RowIndex = rowIndex;
        ValuedVariables = valuedVariables;
        Results = results;
    }

    public int CountExpressionValidationFailed()
    {
        return Results.Aggregate<ExpressionEvaluationResult, int>(0, (total, r) => !r.IsValid ? total += 1 : total);
    }

}
