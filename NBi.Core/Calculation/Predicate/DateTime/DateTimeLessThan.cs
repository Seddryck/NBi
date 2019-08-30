using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeLessThan : DateTimePredicate
    {
        public DateTimeLessThan(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool Compare(System.DateTime x, System.DateTime y)
        {
            return x < y;
        }
        public override string ToString()
        {
            return $"is before {Reference.Execute()}";
        }
    }
}
