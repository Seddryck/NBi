using NBi.Core.ResultSet.Resolver;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

class ProjectionResultSetScalarResolver<T> : IScalarResolver<T>
{
    private readonly ProjectionResultSetScalarResolverArgs args;
    private readonly ResultSetResolverFactory factory;
    public ProjectionResultSetScalarResolver(ProjectionResultSetScalarResolverArgs args, ResultSetResolverFactory factory)
    {
        this.args = args;
        this.factory = factory;
    }

    public T? Execute()
    {
        var resolver = factory.Instantiate(args.ResultSetArgs);
        var resultSet = resolver.Execute();

        var projectionResult = args.Projection(resultSet);

        return (T?)Convert.ChangeType(projectionResult, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}
