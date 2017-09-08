using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericWithinRange : AbstractPredicateReference
    {
        public NumericWithinRange(object reference) : base(reference)
        { }

        public override bool Apply(object x)
        {
            var builder = new IntervalBuilder(Reference);
            builder.Build();
            var interval = builder.GetInterval();

            var predicates = new List<NumericPredicate>();
            if (interval.Left.IsOpen)
                predicates.Add(new NumericMoreThan(interval.Left.Value));
            else if (!(interval.Left is LeftEndPointNegativeInfinity))
                predicates.Add(new NumericMoreThanOrEqual(interval.Left.Value));

            if (interval.Right.IsOpen)
                predicates.Add(new NumericLessThan(interval.Right.Value));
            else if (!(interval.Right is RightEndPointPositiveInfinity))
                predicates.Add(new NumericLessThanOrEqual(interval.Right.Value));

            return predicates.Aggregate(true, (result, next) => result && next.Apply(x));
        }
    }
}
