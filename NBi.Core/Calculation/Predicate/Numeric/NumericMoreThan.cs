using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericMoreThan : NumericPredicate
    {
        public override bool Compare(decimal x, decimal y)
        {
            return x > y;
        }
    }
}
