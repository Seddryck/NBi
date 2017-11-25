using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public abstract class ProjectionResultSetScalarResolverArgs : IScalarResolverArgs
    {
        public Func<ResultSet.ResultSet, object> Projection { get; }
        public ResultSetResolverArgs ResultSetArgs { get; }

        public ProjectionResultSetScalarResolverArgs(Func<ResultSet.ResultSet, object> projection, ResultSetResolverArgs resultSetArgs)
        {
            Projection = projection;
            ResultSetArgs = resultSetArgs;
        }
    }
}
