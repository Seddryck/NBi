using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Boolean
{
    class BooleanEqual : AbstractPredicateReference
    {
        public BooleanEqual(object reference) : base(reference)
        { }

        public override bool Apply(object x)
        {
            var cpr = new BooleanComparer();
            return cpr.Compare(x, Reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to {Reference}";
        }
    }
}
