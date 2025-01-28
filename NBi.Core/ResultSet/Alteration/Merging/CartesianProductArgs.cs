using NBi.Core.ResultSet.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging;

public class CartesianProductArgs : IMergingArgs
{
    public IResultSetResolver ResultSetResolver { get; }

    public CartesianProductArgs(IResultSetResolver resultSetResolver)
        => (ResultSetResolver) = (resultSetResolver);
}
