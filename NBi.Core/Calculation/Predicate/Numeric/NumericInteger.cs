using NBi.Core.ResultSet.Comparer;
using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericInteger : IPredicate
    {
        public bool Apply(object x)
        {
            var converter = new NumericConverter();
            var numX = converter.Convert(x);

            return numX % 1 == 0;
        }

        public override string ToString()
        {
            return $"is an integer";
        }
    }
}
