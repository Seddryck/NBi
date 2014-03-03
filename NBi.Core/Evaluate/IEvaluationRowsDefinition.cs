using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Evaluate
{
    public interface IEvaluationRowsDefinition
    {
        List<IColumnVariable> Variables { get; }
        List<IColumnExpression> Expressions { get; }
    }
}
