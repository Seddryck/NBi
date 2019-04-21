using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractPredicateReference : AbstractPredicate
    {
        public IScalarResolver Reference { get; set; }

        public AbstractPredicateReference(bool not, IScalarResolver reference)
            : base(not)
        {
            Reference = reference;
        }

        protected override bool Apply(object x)
        {
            //if (Reference is ITestVariable)
            //    return ApplyWithReference((Reference as ITestVariable).GetValue(), x);
            //else
                return ApplyWithReference(Reference.Execute(), x);
        }

        protected abstract bool ApplyWithReference(object reference, object x);
    }
}
