using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    abstract class NumericPredicate : AbstractPredicateReference
    {
        public NumericPredicate(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var caster = new NumericCaster();
            var numX = caster.Execute(x);
            var numY = caster.Execute(Reference);

            return Compare(numX, numY);
        }

        protected abstract bool Compare(decimal x, decimal y);
    }
}
