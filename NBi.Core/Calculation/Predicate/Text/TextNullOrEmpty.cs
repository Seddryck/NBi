using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextNullOrEmpty : IPredicate
    {
        public bool Apply(object x)
        {
            var nullPredicate = new TextNull();
            var emptyPredicate = new TextEmpty();
            return (nullPredicate.Apply(x) || emptyPredicate.Apply(x));
        }
        public override string ToString()
        {
            return $"is null or empty";
        }
    }
}
