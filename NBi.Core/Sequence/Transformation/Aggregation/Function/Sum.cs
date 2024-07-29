using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Sum<T> : BaseAggregation<T>
    {
        public Sum(ICaster<T> caster) : base(caster)
        { }

        protected override T? Execute(Series<int, T>? series) => Caster.Execute(series.Sum());
    }

    class SumNumeric : Sum<decimal>
    {
        public SumNumeric() : base(new NumericCaster())
        { }
    }
}
