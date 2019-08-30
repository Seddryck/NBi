using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deedle;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NBi.Core.Sequence.Transformation.Aggregation.Function;
using System.Reflection;

namespace NBi.Core.Sequence.Transformation.Aggregation
{
    public class AggregationFactory
    {
        public Aggregation Instantiate(ColumnType columnType, AggregationFunctionType function, IAggregationStrategy[] strategies)
        {
            var missingValue = (IMissingValueStrategy)(strategies.SingleOrDefault(x => x is IMissingValueStrategy) ?? new DropStrategy());
            var emptySeries = (IEmptySeriesStrategy)(strategies.SingleOrDefault(x => x is IEmptySeriesStrategy) ?? new ReturnDefaultStrategy(0));

            var @namespace = $"{this.GetType().Namespace}.Function.";
            var typeName = $"{Enum.GetName(typeof(AggregationFunctionType), function)}{Enum.GetName(typeof(ColumnType), columnType)}";
            var type = GetType().Assembly.GetType($"{@namespace}{typeName}", false, true) ?? throw new ArgumentException($"No aggregation named '{typeName}' has been found in the namespace '{@namespace}'.");
            return new Aggregation((IAggregationFunction)(type.GetConstructor(Type.EmptyTypes).Invoke(new object[] { })), missingValue, emptySeries);
        }

        public Aggregation Instantiate(AggregationArgs args)
            => Instantiate(args.ColumnType, args.Function, args.Strategies.ToArray());
    }
}
