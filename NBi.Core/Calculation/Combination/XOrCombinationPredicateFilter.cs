using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Combination
{
    class XOrCombinationPredicateFilter : BaseCombinationPredicateFilter
    {
        public override string Description { get => "or"; }

        public XOrCombinationPredicateFilter(ServiceLocator serviceLocator, Context context, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IEnumerable<Predication> predications)
            : base(serviceLocator, context, aliases, expressions, predications)
        { }

        protected override bool RowApply(Context context)
        {
            var result = false;
            foreach (var predication in predications)
            {
                var value = GetValueFromRow(context, predication.Operand);
                result ^= predication.Predicate.Execute(value);
            }
            return result;
        }
        
    }
}
