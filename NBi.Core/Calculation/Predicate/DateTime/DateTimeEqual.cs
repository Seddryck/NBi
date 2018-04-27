using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeEqual : AbstractPredicateReference
    {
        public DateTimeEqual(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var cpr = new DateTimeComparer();
            return cpr.Compare(x, Reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to {Reference}";
        }
    }
}
