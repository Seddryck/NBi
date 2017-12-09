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
    class AndCombinationPredicateFilter : BaseCombinationPredicateFilter
    {
        public override string Description { get => "and"; }

        public AndCombinationPredicateFilter(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IEnumerable<Predication> predications)
            : base(aliases, expressions, predications)
        { }

        protected override bool RowApply(DataRow row)
        {
            var result = true;
            foreach (var predication in predications)
            {
                var value = GetValueFromRow(row, predication.Operand);
                result &= predication.Predicate.Apply(value);
            }
            return result;
        }
        
    }
}
