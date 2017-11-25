using NBi.Core.Query;
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
        
        protected virtual IDbCommand ResolveQuery()
        {
            var factory = new QueryResolverFactory();
            var resolver = factory.Instantiate(args.QueryArgs);
            var cmd = resolver.Execute();
            return cmd;
        }

        protected virtual object ExecuteQuery(IDbCommand command)
        {
            var factory = new QueryEngineFactory();
            var queryEngine = factory.GetExecutor(command);
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
