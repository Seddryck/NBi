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
    public class PredicateFilterFactory
    {
        public PredicateFilter Instantiate(IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IPredicateInfo predicateInfo)
        {
            var factory = new PredicateFactory();
            var predicate = factory.Get(predicateInfo);

            var pf = new PredicateFilter(aliases, expressions, predicateInfo.Name, predicate.Apply, predicate.ToString);

            return pf;
        }
    }
}
