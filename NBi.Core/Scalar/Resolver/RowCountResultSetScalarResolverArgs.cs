using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public class RowCountResultSetScalarResolverArgs : ProjectionResultSetScalarResolverArgs
    {
        public RowCountResultSetScalarResolverArgs(ResultSetResolverArgs resultSetArgs)
            : base((ResultSet.ResultSet rs) => rs.Rows.Count, resultSetArgs)
        {
        }
    }
}
