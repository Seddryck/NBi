using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextNull : IPredicate
    {
        public bool Apply(object x)
        {
            return x == null || x == DBNull.Value || (x as string) == "(null)";
        }
        public override string ToString()
        {
            return $"is null";
        }
    }
}
