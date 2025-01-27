using Deedle;
using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Function;

abstract class BaseAggregation<T> : IAggregationFunction
{
    protected ICaster<T> Caster { get; }

    public BaseAggregation(ICaster<T> caster) => Caster = caster;

    public object? Execute(IEnumerable<object> values)
    {
        var series = values.Select(x => Caster.Execute(x)).ToOrdinalSeries();
        return Execute(series);
    }

    protected abstract T? Execute(Series<int, T>? series);
}
