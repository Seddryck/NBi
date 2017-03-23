using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractPredicateReference : IPredicate
    {
        public abstract bool Apply(object x);
        
        public object Reference { get; set; }

        public AbstractPredicateReference(object reference)
        {
            Reference = reference;
        }
    }
}
