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

        public AbstractTextPredicate(object reference, StringComparison stringComparison) 
            : base(reference)
        {
            this.StringComparison = stringComparison;
        }
    }
}
