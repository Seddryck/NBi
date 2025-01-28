using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.Scalar.Resolver;

public class RowCountResultSetScalarResolverArgs : ProjectionResultSetScalarResolverArgs
{
    public RowCountResultSetScalarResolverArgs(ResultSetResolverArgs resultSetArgs)
        : base((IResultSet rs) => rs.RowCount, resultSetArgs)
    {
    }
}
