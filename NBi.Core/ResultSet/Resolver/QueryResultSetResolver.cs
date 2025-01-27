using NBi.Core.Injection;
using NBi.Extensibility;
using NBi.Core.Query.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility.Resolving;

namespace NBi.Core.ResultSet.Resolver;

class QueryResultSetResolver : IResultSetResolver
{
    private readonly QueryResultSetResolverArgs args;
    private readonly ServiceLocator serviceLocator;

    public QueryResultSetResolver(QueryResultSetResolverArgs args, ServiceLocator serviceLocator)
    {
        this.args = args;
        this.serviceLocator = serviceLocator;
    }
    
    public IResultSet Execute()
    {
        var cmd = Resolve();
        var rs = Load(cmd);
        return rs;
    }

    protected virtual IQuery Resolve()
    {
        var factory = serviceLocator.GetQueryResolverFactory();
        var resolver = factory.Instantiate(args.QueryResolverArgs as BaseQueryResolverArgs);
        var query = resolver.Execute();
        return query;
    }

    protected virtual IResultSet Load(IQuery query)
    {
        var factory = serviceLocator.GetExecutionEngineFactory();
        var qe = factory.Instantiate(query);
        var ds = qe.Execute();
        return new DataTableResultSet(ds.Tables[0]);
    }
}
