using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    abstract class NumericPredicate : IPredicate
    {
        public bool Compare(object x, object y)
        {
            var converter = new NumericConverter();
            var numX = converter.Convert(x);
            var numY = converter.Convert(y);

            return Compare(numX, numY);
        }

        public abstract bool Compare(decimal x, decimal y);
    }
}
