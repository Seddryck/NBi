using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

class QueryResolver : IQueryResolver
{
    private readonly QueryResolverArgs args;

    public QueryResolver(QueryResolverArgs args)
    {
        this.args = args;
    }

    public IQuery Execute()
    {
        return new Query(args.Statement, args.ConnectionString, args.Timeout, args.Parameters, args.Variables);
    }
}
