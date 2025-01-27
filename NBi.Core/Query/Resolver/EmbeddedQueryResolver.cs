using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

class EmbeddedQueryResolver : IQueryResolver
{
    private readonly EmbeddedQueryResolverArgs args;

    public EmbeddedQueryResolver(EmbeddedQueryResolverArgs args)
    {
        this.args = args;
    }

    public IQuery Execute()
    {
        var query = new Query(args.CommandText, args.ConnectionString, args.Timeout, args.Parameters, args.Variables);
        return query;
    }
}
