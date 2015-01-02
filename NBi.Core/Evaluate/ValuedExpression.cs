using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;

namespace NBi.Core.Evaluate
{
    public class ValuedExpression : ExpressionComparable
    {
        public Object Value { get; set; }
        public ColumnType Type { get; set; }
        public string Tolerance { get; set; }

        public ValuedExpression(string expression, object value, ColumnType type, string tolerance)
            :base(expression)
        {
            Value = value;
            Type = type;
            Tolerance = tolerance;
        }

        public ValuedExpression(string expression, object value, decimal tolerance)
            : this (expression, value, ColumnType.Numeric, Convert.ToString(tolerance))
        {
        }

        public ValuedExpression(string expression, object value)
            : this(expression, value, ColumnType.Numeric, "0")
        {
        }

        public ValuedExpression(string expression, object value, TimeSpan tolerance)
            : this(expression, value, ColumnType.DateTime, Convert.ToString(tolerance))
        {
        }

        public ExpressionEvaluationResult Compare(Dictionary<string, Object> variables)
        {
            this.Parse();
            var actualValue = this.Evaluate(variables);

            var isValid = this.Comparer.Compare(actualValue, Value, Type, Tolerance);

            return new ExpressionEvaluationResult(Sentence, isValid, actualValue, Value);
        }
    }
}
