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
    class NumericInteger : AbstractPredicate
    {
        public NumericInteger(bool not)
            : base(not)
        { }

        protected override bool Apply(object x)
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
