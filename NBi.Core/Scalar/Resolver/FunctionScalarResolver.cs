using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class FunctionScalarResolver<T> : IScalarResolver<T>
{
    protected internal FunctionScalarResolverArgs Args { get; }

    public FunctionScalarResolver(FunctionScalarResolverArgs args) => Args = args;

    public T? Execute()
    {
        var value = Args.Resolver.Execute();
        foreach (var transformation in Args.Transformations)
            value = transformation.Evaluate(value);
        return (T?)value;
    }

    object? IResolver.Execute() => Execute();
}
