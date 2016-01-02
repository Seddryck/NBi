using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Boolean
{
    class BooleanEqual : IPredicate
    {
        public bool Compare(object x, object y)
        {
            var cpr = new BooleanComparer();
            return cpr.Compare(x, y).AreEqual;
        }
    }
}
