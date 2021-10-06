using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractPredicateReference : AbstractPredicate
    {
        public IResolver Reference { get; set; }

        public AbstractPredicateReference(bool not, IResolver reference)
            : base(not)
        {
            Reference = reference;
        }

        protected override bool Apply(object x) => ApplyWithReference(Reference.Execute(), x);

        protected abstract bool ApplyWithReference(object reference, object x);
    }
}
