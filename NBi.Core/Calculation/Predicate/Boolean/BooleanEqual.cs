using NBi.Core.Scalar.Comparer;
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
        public BooleanEqual(bool not, object reference) : base(not, reference)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var cpr = new BooleanComparer();
            return cpr.Compare(x, reference).AreEqual;
        }

        public override string ToString() => $"is equal to {Reference}";
    }
}
