using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericEqual : IPredicate
    {
        public bool Compare(object x, object y)
        {
            var comparer = new NumericComparer();
            return comparer.Compare(x, y).AreEqual;
        }
    }
}
