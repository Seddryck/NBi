using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class VariableFactory
    {
        public IVariable Instantiate(VariableScope scope, IScalarResolver resolver)
        {
            switch (scope)
            {
                case VariableScope.Global: return new GlobalVariable(resolver);
                case VariableScope.Local:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
