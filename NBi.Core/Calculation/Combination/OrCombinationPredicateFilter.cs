using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Combination
{
    class OrCombinationPredicateFilter : BaseCombinationPredicateFilter
    {
        public override string Description { get => "or"; }

        public OrCombinationPredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IEnumerable<Predication> predications)
            : base(aliases, expressions, predications)
        { }

        protected override bool RowApply(DataRow row)
        {
            var result = false;
            foreach (var predication in predications)
            {
                var value = GetValueFromRow(row, predication.Operand);
                result |= predication.Predicate.Apply(value);
            }
            return result;
        }
        
    }
}
