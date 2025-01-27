using NBi.Core.ResultSet.Alteration;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class AlterationResultSetResolverArgs : ResultSetResolverArgs
{
    public IResultSetResolver Resolver { get; }
    public IEnumerable<IAlteration> Alterations { get; }

    public AlterationResultSetResolverArgs(IResultSetResolver resolver, IEnumerable<IAlteration> alterations)
        => (Resolver, Alterations) = (resolver, alterations);
}
