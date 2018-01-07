using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet.Interval;
using NBi.Core.ResultSet.Caster;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericWithinRange : AbstractPredicateReference
    {
        public NumericWithinRange(bool not, object reference) : base(not, reference)
        { }

        protected override bool Apply(object x)
        {
            var builder = new NumericIntervalBuilder(Reference);
            builder.Build();
            var interval = builder.GetInterval();

            var caster = new NumericCaster();
            var numX = caster.Execute(x);
            return interval.Contains(numX);
        }

        public override string ToString() => $"is within the interval {Reference}";
    }
}
