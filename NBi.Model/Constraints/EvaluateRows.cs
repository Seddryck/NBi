using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Evaluate;
using NBi.Model.Items.Calculation;

namespace NBi.Model.Constraints;

public class EvaluateRows: AbstractConstraint, IEvaluationRowsDefinition
{
    public List<IColumnAlias> Variables
    {
        get
        {
            return VariablesInternal.ToList< IColumnAlias>();
        }
    }

    public List<IColumnExpression> Expressions
    {
        get
        {
            return ExpressionsInternal.ToList<IColumnExpression>();
        }
    }

    public List<Alias> VariablesInternal { get; set; }

    public List<Expression> ExpressionsInternal { get; set; }
}
