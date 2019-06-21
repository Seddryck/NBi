using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Mean<T> : BaseNumericAggregation<T>
    {
        public Mean(ICaster<T> caster) : base(caster)
        { }

        protected override T Execute(Series<int, T> series) => Caster.Execute(series.Mean());
    }

    class MeanNumeric : Mean<decimal>
    {
        public MeanNumeric() : base(new NumericCaster())
        { }
    }
}
