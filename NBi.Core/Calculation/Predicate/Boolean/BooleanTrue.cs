using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Boolean
{
    class BooleanTrue : IPredicate
    {
        public bool Apply(object x)
        {
            var cpr = new BooleanComparer();
            return cpr.Compare(x, true).AreEqual;
        }
    }
}
