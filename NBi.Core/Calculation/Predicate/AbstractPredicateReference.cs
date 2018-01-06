using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractPredicateReference : AbstractPredicate
    {
        public object Reference { get; set; }

        public AbstractPredicateReference(bool not, object reference)
            : base(not)
        {
            Reference = reference;
        }
    }
}
