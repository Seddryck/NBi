using Deedle;
using NBi.Core.ResultSet;
using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace NBi.Core.Sequence.Transformation.Aggregation;

public class AggregationFactory
{
    public Aggregation Instantiate(ColumnType columnType, AggregationFunctionType function, IScalarResolver[] parameters, IAggregationStrategy[] strategies)
    {
        var missingValue = (IMissingValueStrategy)(strategies.SingleOrDefault(x => x is IMissingValueStrategy) ?? new DropStrategy(columnType));
        var emptySeries = (IEmptySeriesStrategy)(strategies.SingleOrDefault(x => x is IEmptySeriesStrategy) ?? new ReturnDefaultStrategy(columnType));

        var @namespace = $"{this.GetType().Namespace}.Function.";
        var typeName = $"{Enum.GetName(typeof(AggregationFunctionType), function)}{Enum.GetName(typeof(ColumnType), columnType)}";
        var type = GetType().Assembly.GetType($"{@namespace}{typeName}", false, true) ?? throw new ArgumentException($"No aggregation named '{typeName}' has been found in the namespace '{@namespace}'.");

        if ((parameters?.Length ?? 0) == 0)
            return new Aggregation((IAggregationFunction)(type.GetConstructor(Type.EmptyTypes) ?? throw new NullReferenceException()).Invoke([]), missingValue, emptySeries);
        else
        {
            var ctor = type.GetConstructors().Where(x => x.IsPublic && (x.GetParameters().Length == parameters!.Length)).FirstOrDefault()
                ?? throw new ArgumentException($"No public constructor for the aggregation '{function}' expecting {parameters!.Length} parameters.");
            return new Aggregation((IAggregationFunction)ctor.Invoke(parameters), missingValue, emptySeries);
        }
    }

    public Aggregation Instantiate(AggregationArgs args)
        => Instantiate(args.ColumnType, args.Function, [.. args.Parameters], [.. args.Strategies]);
}
