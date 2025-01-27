using NBi.Core.ResultSet.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public abstract class ProjectionResultSetScalarResolverArgs : IScalarResolverArgs
{
    public Func<IResultSet, object> Projection { get; }
    public ResultSetResolverArgs ResultSetArgs { get; }

    public ProjectionResultSetScalarResolverArgs(Func<IResultSet, object> projection, ResultSetResolverArgs resultSetArgs)
    {
        Projection = projection;
        ResultSetArgs = resultSetArgs;
    }
}
