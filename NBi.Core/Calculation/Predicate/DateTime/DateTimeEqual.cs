using NBi.Core.Scalar.Comparer;
using NBi.Extensibility.Resolving;
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
        public DateTimeEqual(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var cpr = new DateTimeComparer();
            return cpr.Compare(x, reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to {Reference}";
        }
    }
}
