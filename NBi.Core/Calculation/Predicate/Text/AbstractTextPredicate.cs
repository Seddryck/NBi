using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractTextPredicate : AbstractPredicateReference
    {
        public StringComparison StringComparison { get; private set; }

        public AbstractTextPredicate(bool not, IResolver reference, StringComparison stringComparison) 
            : base(not, reference)
        {
            this.StringComparison = stringComparison;
        }
    }
}
