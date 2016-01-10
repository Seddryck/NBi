using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMoreThanOrEqual : IPredicate
    {
        public bool Compare(object x, object y)
        {
            var cpr = StringComparer.Create(CultureInfo.InvariantCulture, false);
            return cpr.Compare(x.ToString(), y.ToString()) >= 0;
        }
    }
}
