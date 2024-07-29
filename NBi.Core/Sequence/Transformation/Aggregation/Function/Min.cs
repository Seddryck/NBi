using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function
{
    abstract class Min<T> : BaseAggregation<T>
    {
        public Min(ICaster<T> caster) : base(caster)
        { }

        protected override T? Execute(Series<int, T>? series) => Caster.Execute(series.Min());
    }

    class MinNumeric : Min<decimal>
    {
        public MinNumeric() : base(new NumericCaster())
        { }
    }

    class MinDateTime : Min<DateTime>
    {
        public MinDateTime() : base(new DateTimeCaster())
        { }
        protected override DateTime Execute(Series<int, DateTime> series) => Caster.Execute(series.TryMin().Value);
    }
}
