using NBi.Core.Scalar.Comparer;
using NBi.Core.Scalar.Caster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericInteger : AbstractPredicate
    {
        public NumericInteger(bool not)
            : base(not)
        { }

        protected override bool Apply(object x)
        {
            var caster = new NumericCaster();
            var numX = caster.Execute(x);

            return numX % 1 == 0;
        }

        public override string ToString()
        {
            return $"is an integer";
        }
    }
}
