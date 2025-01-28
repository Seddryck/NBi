using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class EnvironmentScalarResolver<T> : IScalarResolver<T>
{
    private readonly EnvironmentScalarResolverArgs args;

    public EnvironmentScalarResolver(EnvironmentScalarResolverArgs args)
    {
        this.args = args;
    }

    public T? Execute()
    {
        var value = Environment.GetEnvironmentVariable(args.Name, EnvironmentVariableTarget.Process)
                        ?? Environment.GetEnvironmentVariable(args.Name, EnvironmentVariableTarget.User)
                        ?? Environment.GetEnvironmentVariable(args.Name, EnvironmentVariableTarget.Machine);
        return (T?)Convert.ChangeType(value, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}
