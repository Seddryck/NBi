using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class QueryResultSetResolverArgs : ResultSetResolverArgs
    {
        public QueryResolverArgs QueryResolverArgs { get; }

        public QueryResultSetResolverArgs(QueryResolverArgs args)
        {
            QueryResolverArgs = args;
        }
    }
}