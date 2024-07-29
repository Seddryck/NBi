using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public class GlobalVariableScalarResolverArgs : IScalarResolverArgs
    {
        public string VariableName { get; }
        public Context Context { get; }

        public GlobalVariableScalarResolverArgs(string variableName, Context context)
            => (VariableName, Context) = (variableName, context);
    }
}
