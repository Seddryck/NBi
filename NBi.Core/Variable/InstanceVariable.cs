using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class InstanceVariable : ITestVariable
    {
        private readonly object value;

        public InstanceVariable(object value)
        {
            this.value = value;
        }

        public object GetValue() => value;

        public bool IsEvaluated() => true;
    }
}
