using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public class LiteralScalarResolverArgs : IScalarResolverArgs
{
    public object Object { get; }

    public LiteralScalarResolverArgs(object @object)
    {
        this.Object = @object;
    }
}
