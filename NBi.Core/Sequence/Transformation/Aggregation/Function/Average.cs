using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Average<T>(ICaster<T> caster) : BaseAggregation<T>(caster)
    {
        protected override T? Execute(Series<int, T>? series) => Caster.Execute(series.Mean());
    }

    class AverageNumeric : Average<decimal>
    {
        public AverageNumeric() : base(new NumericCaster())
        { }
    }
}
