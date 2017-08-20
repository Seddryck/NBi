using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    abstract class NumericPredicate : AbstractPredicateReference
    {
        public NumericPredicate(object reference) : base(reference)
        { }

        public override bool Apply(object x)
        {
            var converter = new NumericConverter();
            var numX = converter.Convert(x);
            var numY = converter.Convert(Reference);

            return Compare(numX, numY);
        }

        protected abstract bool Compare(decimal x, decimal y);
    }
}
