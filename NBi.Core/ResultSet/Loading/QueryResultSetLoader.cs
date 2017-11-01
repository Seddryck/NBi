using NBi.Core.Query;
using NBi.Core.ResultSet.Resolver.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Loading
{
    class QueryResultSetLoader : IResultSetLoader
    {
        private readonly QueryResolverArgs args;

        public QueryResultSetLoader(QueryResolverArgs args)
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
            var resolver = factory.Instantiate(args);
            var cmd = resolver.Execute();
            return cmd;
        }

        protected virtual ResultSet Load(IDbCommand command)
        {
            var qe = new QueryEngineFactory().GetExecutor(command);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }
    }
}
