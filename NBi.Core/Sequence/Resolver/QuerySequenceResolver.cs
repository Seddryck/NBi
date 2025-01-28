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
using System.Collections;

namespace NBi.Core.Sequence.Resolver;

class QuerySequenceResolver<T> : ISequenceResolver<T>
{
    private QuerySequenceResolverArgs Args { get; }
    private ServiceLocator ServiceLocator { get; }

    public QuerySequenceResolver(QuerySequenceResolverArgs args, ServiceLocator serviceLocator)
     => (Args, ServiceLocator) = (args, serviceLocator);
    
    protected virtual IQuery ResolveQuery()
    {
        var factory = ServiceLocator.GetQueryResolverFactory();
        var resolver = factory.Instantiate(Args.QueryArgs);
        var query = resolver.Execute();
        return query;
    }

    protected virtual IEnumerable<T> ExecuteQuery(IQuery query)
    {
        var factory = ServiceLocator.GetExecutionEngineFactory();
        var queryEngine = factory.Instantiate(query);
        var value = queryEngine.ExecuteList<T>();
        return value;
    }

    public List<T> Execute()
    {
        var cmd = ResolveQuery();
        var value = ExecuteQuery(cmd);
        return value.ToList();
    }

    object IResolver.Execute() => Execute();
    IList ISequenceResolver.Execute() => Execute();
}
