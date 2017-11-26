using Microsoft.CSharp;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class TestVariable : ITestVariable
    {
        private object value;
        private bool isEvaluated;
        private readonly IScalarResolver<object> resolver;
        
        public TestVariable(IScalarResolver<object> resolver)
        {
            this.resolver = resolver;
        }

        public object GetValue()
        {
            if (!IsEvaluated())
            {
                value = resolver.Execute();
                isEvaluated = true;
            }

            return value;
        }

        public bool IsEvaluated()
        {
            return isEvaluated;
        }
    }
}
