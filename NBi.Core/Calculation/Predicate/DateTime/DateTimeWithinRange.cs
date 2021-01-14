using NBi.Core.Scalar.Interval;
using NBi.Core.Scalar.Casting;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeWithinRange : AbstractPredicateReference
    {
        public DateTimeWithinRange(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var builder = new DateTimeIntervalBuilder(reference);
            builder.Build();
            var interval = builder.GetInterval();

            var caster = new DateTimeCaster();
            var dtX = caster.Execute(x);
            return interval.Contains(dtX);
        }

        public override string ToString()
        {
            return $"is within the interval {Reference.Execute()}";
        }
    }
}
