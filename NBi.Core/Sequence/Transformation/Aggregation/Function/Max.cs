using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Max<T> : BaseAggregation<T>
    {
        public Max(ICaster<T> caster) : base(caster)
        { }

        protected override T? Execute(Series<int, T>? series) => Caster.Execute(series.Max());
    }

    class MaxNumeric : Max<decimal>
    {
        public MaxNumeric() : base(new NumericCaster())
        { }
    }

    class MaxDateTime : Max<DateTime>
    {
        public MaxDateTime() : base(new DateTimeCaster())
        { }
        protected override DateTime Execute(Series<int, DateTime>? series) 
            => Caster.Execute(series.TryMax().Value);
    }
}
