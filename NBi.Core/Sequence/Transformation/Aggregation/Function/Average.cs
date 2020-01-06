using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Average<T> : BaseAggregation<T>
    {
        public Average(ICaster<T> caster) : base(caster)
        { }

        protected override T Execute(Series<int, T> series) => Caster.Execute(series.Mean());
    }

    class AverageNumeric : Average<decimal>
    {
        public AverageNumeric() : base(new NumericCaster())
        { }
    }
}
