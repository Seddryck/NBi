using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public class ContextScalarResolverArgs : IScalarResolverArgs
{
    public Context Context { get; }
    public IColumnIdentifier ColumnIdentifier { get; }

    public ContextScalarResolverArgs(Context context, IColumnIdentifier columnIdentifier)
        => (Context, ColumnIdentifier) = (context, columnIdentifier);
}
