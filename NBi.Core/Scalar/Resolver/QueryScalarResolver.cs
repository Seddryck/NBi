using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class QueryScalarResolver<T> : IScalarResolver<T>
    {
        private readonly QueryScalarResolverArgs args;

        public QueryScalarResolver(QueryScalarResolverArgs args)
        {
            this.args = args;
        }
        
        protected virtual IQuery ResolveQuery()
        {
            var factory = new QueryResolverFactory();
            var resolver = factory.Instantiate(args.QueryArgs);
            var query = resolver.Execute();
            return query;
        }

        protected virtual object ExecuteQuery(IQuery query)
        {
            var factory = new ExecutionEngineFactory();
            var queryEngine = factory.Instantiate(query);
            var value = queryEngine.ExecuteScalar();
            return value;
        }

        public T Execute()
        {
            var cmd = ResolveQuery();
            var value = ExecuteQuery(cmd);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
