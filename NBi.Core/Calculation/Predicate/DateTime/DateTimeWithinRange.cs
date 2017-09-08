using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet.Interval;
using NBi.Core.ResultSet.Converter;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeWithinRange : AbstractPredicateReference
    {
        public DateTimeWithinRange(object reference) : base(reference)
        { }

        public override bool Apply(object x)
        {
            var builder = new DateTimeIntervalBuilder(Reference);
            builder.Build();
            var interval = builder.GetInterval();

            var converter = new DateTimeConverter();
            var dtX = converter.Convert(x);
            return interval.Contains(dtX);
        }
    }
}
