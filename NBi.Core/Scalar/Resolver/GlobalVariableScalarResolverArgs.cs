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
        public IDictionary<string, ITestVariable> GlobalVariables { get; }

        public GlobalVariableScalarResolverArgs(string variableName, IDictionary<string, ITestVariable> globalVariables)
        {
            this.VariableName = variableName;
            this.GlobalVariables = globalVariables;
        }
        
    }
}
