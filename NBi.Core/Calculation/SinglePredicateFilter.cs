using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
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
        private readonly IColumnIdentifier operand;
        private readonly Func<string> describeFunction;

        public SinglePredicateFilter(ServiceLocator serviceLocator, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IColumnIdentifier operand, Func<object, bool> implementation, Func<string> describeFunction)
            : base(serviceLocator, aliases, expressions)
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
            return $"{operand.Label} {describeFunction()}.";
        }
    }
}
