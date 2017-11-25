using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    class ProjectionResultSetScalarResolver<T> : IScalarResolver<T>
    {
        private readonly ProjectionResultSetScalarResolverArgs args;
        public ProjectionResultSetScalarResolver(ProjectionResultSetScalarResolverArgs args)
        {
            this.args = args;
        }

        public T Execute()
        {
            var factory = new ResultSetResolverFactory();
            var resolver = factory.Instantiate(args.ResultSetArgs);
            var resultSet = resolver.Execute();

            var projectionResult = args.Projection(resultSet);

            return (T)Convert.ChangeType(projectionResult, typeof(T));
        }
    }
}
