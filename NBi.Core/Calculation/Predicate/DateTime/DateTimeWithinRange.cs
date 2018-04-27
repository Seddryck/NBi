using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Scalar.Interval;
using NBi.Core.Scalar.Caster;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeWithinRange : AbstractPredicateReference
    {
        public DateTimeWithinRange(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var builder = new DateTimeIntervalBuilder(Reference);
            builder.Build();
            var interval = builder.GetInterval();

            var caster = new DateTimeCaster();
            var dtX = caster.Execute(x);
            return interval.Contains(dtX);
        }

        public override string ToString()
        {
            return $"is within the interval {Reference}";
        }
    }
}
