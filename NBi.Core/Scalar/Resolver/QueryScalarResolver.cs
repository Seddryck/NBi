using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Scalar.Resolver;

class QueryScalarResolver<T> : IScalarResolver<T>
{
    private readonly QueryScalarResolverArgs args;
    private readonly ServiceLocator serviceLocator;

    public QueryScalarResolver(QueryScalarResolverArgs args, ServiceLocator serviceLocator)
    {
        this.args = args;
        this.serviceLocator = serviceLocator;
    }
    
    protected virtual IQuery ResolveQuery()
    {
        var factory = serviceLocator.GetQueryResolverFactory();
        var resolver = factory.Instantiate(args.QueryArgs);
        var query = resolver.Execute();
        return query;
    }

    protected virtual object? ExecuteQuery(IQuery query)
    {
        var factory = serviceLocator.GetExecutionEngineFactory();
        var queryEngine = factory.Instantiate(query);
        var value = queryEngine.ExecuteScalar();
        return value;
    }

    public T? Execute()
    {
        var cmd = ResolveQuery();
        var value = ExecuteQuery(cmd);
        return (T?)Convert.ChangeType(value, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}
