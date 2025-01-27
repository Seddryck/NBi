using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using NBi.Core.Variable;

namespace NBi.Core.Scalar.Resolver;

class NCalcScalarResolverArgs : IScalarResolverArgs
{
    public string Code { get; }
    public Context Context { get; }

    public NCalcScalarResolverArgs(string code, Context context)
     => (Code, Context) = (code, context);

}
