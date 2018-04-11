using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    class SinglePredicateFilter : BasePredicateFilter
    {
        private readonly Func<object, bool> implementation;
        private readonly string operand;
        private readonly Func<string> describeFunction;

        public SinglePredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, string operand, Func<object, bool> executeFunction)
            : this (aliases, expressions, operand, executeFunction, () => "unspecified description")
        { }

        public SinglePredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, string operand, Func<object, bool> implementation, Func<string> describeFunction)
            : base(aliases, expressions)
        {
            this.operand = operand;
            this.implementation = implementation;
            this.describeFunction = describeFunction;
        }
        
        protected override bool RowApply(DataRow row)
        {
            var value = GetValueFromRow(row, operand);
            return implementation(value);
        }

        

        public override string Describe()
        {
            return $"{operand} {describeFunction()}.";
        }
    }
}
