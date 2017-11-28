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

        protected virtual IDbCommand Resolve()
        {
            var factory = new QueryResolverFactory();
            var resolver = factory.Instantiate(args.QueryResolverArgs as QueryResolverArgs);
            var cmd = resolver.Execute();
            return cmd;
        }

        protected virtual ResultSet Load(IDbCommand command)
        {
            var factory = new ExecutionEngineFactory();
            var qe = factory.Instantiate(command);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }
    }
}
