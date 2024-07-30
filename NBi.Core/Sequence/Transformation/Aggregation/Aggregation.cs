using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation
{
    public class Aggregation
    {
        protected internal IAggregationFunction Function { get; }
        protected internal IMissingValueStrategy MissingValues { get; }
        protected internal IEmptySeriesStrategy EmptySeries { get; }

        public Aggregation(IAggregationFunction function, IMissingValueStrategy missingValue, IEmptySeriesStrategy emptySeries)
            => (Function, MissingValues, EmptySeries) = (function, missingValue, emptySeries);

        public object? Execute(List<object> values)
        {
            var typedValues = MissingValues.Execute(values);
            if (!typedValues.Any())
                return EmptySeries.Execute();
            else
                return Function.Execute(typedValues);
        }
    }
}
