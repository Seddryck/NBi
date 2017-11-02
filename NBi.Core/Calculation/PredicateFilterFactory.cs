using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predicate.Combination;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public class PredicateFilterFactory
    {
        public BasePredicateFilter Instantiate(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IPredicateInfo predicateInfo)
        {
            var factory = new PredicateFactory();
            var predicate = factory.Instantiate(predicateInfo);

            var pf = new SinglePredicateFilter(aliases, expressions, predicateInfo.Name, predicate.Apply, predicate.ToString);

            return pf;
        }

        public BasePredicateFilter Instantiate(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, CombinationOperator combinationOperator, IEnumerable<IPredicateInfo> predicateInfos)
        {
            var predications = new List<Predication>();

            var factory = new PredicateFactory();
            foreach (var predicateInfo in predicateInfos)
            {
                var predicate = factory.Instantiate(predicateInfo);
                predications.Add(new Predication(predicate, predicateInfo.Name));
            }

            switch (combinationOperator)
            {
                case CombinationOperator.Or:
                    return new OrCombinationPredicateFilter(aliases, expressions, predications);
                case CombinationOperator.XOr:
                    return new XOrCombinationPredicateFilter(aliases, expressions, predications);
                case CombinationOperator.And:
                    return new AndCombinationPredicateFilter(aliases, expressions, predications);
                default:
                    throw new ArgumentOutOfRangeException(nameof(combinationOperator));
            }
        }
    }
}
