using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericEqual : AbstractPredicateReference
    {
        public NumericEqual(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var comparer = new NumericComparer();
            return comparer.Compare(x, Reference).AreEqual;
        }
        public override string ToString() => $"is equal to {Reference}";
    }
}
