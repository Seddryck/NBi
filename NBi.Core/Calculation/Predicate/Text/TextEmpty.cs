using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextEmpty : IPredicate
    {
        public bool Apply(object x)
        {
            return (x as string).Length == 0 || (x as string)=="(empty)";
        }


        public override string ToString()
        {
            return $"is empty";
        }
    }
}
