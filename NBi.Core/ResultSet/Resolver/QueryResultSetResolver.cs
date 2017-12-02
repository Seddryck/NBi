using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class QueryResultSetResolver : IResultSetResolver
    {
        private readonly QueryResultSetResolverArgs args;

        public QueryResultSetResolver(QueryResultSetResolverArgs args)
        {
            this.args = args;
        }
        
        public ResultSet Execute()
        {
            var cmd = Resolve();
            var rs = Load(cmd);
            return rs;
        }

        protected virtual IQuery Resolve()
        {
            var factory = new QueryResolverFactory();
            var resolver = factory.Instantiate(args.QueryResolverArgs as BaseQueryResolverArgs);
            var query = resolver.Execute();
            return query;
        }

        protected virtual ResultSet Load(IQuery query)
        {
            var factory = new ExecutionEngineFactory();
            var qe = factory.Instantiate(query);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }
    }
}
