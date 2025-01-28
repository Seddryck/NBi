using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Evaluate;

public class RowValidator
{
    public IEnumerable<ExpressionEvaluationResult> Execute(Dictionary<string, Object> variables, IList<ValuedExpression> expressions)
    {
        var result = new List<ExpressionEvaluationResult>(expressions.Count);
        foreach (var expression in expressions)
        {
            result.Add(expression.Compare(variables));
        }
        return result;
    }        
}
