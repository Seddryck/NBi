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
        public DateTimeLessThan(object reference) : base(reference)
        { }

        protected override bool Compare(System.DateTime x, System.DateTime y)
        {
            return x < y;
        }
        public override string ToString()
        {
            return $"is less than {Reference}";
        }
    }
}
