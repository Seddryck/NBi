using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class IfUnavailableResultSetResolverArgs : ResultSetResolverArgs
{
    public IResultSetResolver Primary { get; }
    public IResultSetResolver Secondary { get; }

    public IfUnavailableResultSetResolverArgs(IResultSetResolver primary, IResultSetResolver secondary)
        => (Primary, Secondary) = (primary, secondary);
}