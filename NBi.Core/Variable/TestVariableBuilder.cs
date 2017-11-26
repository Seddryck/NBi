using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class TestVariableFactory
    {
        public ITestVariable Instantiate(IScalarResolver<object> resolver)
        {
            return new TestVariable(resolver);
        }
    }
}
