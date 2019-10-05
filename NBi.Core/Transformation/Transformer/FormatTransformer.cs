using Microsoft.CSharp;
using NBi.Core.Injection;
using NBi.Core.Scalar.Casting;
using NBi.Core.Variable;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    class FormatTransformer<T> : ITransformer
    {
        private ServiceLocator ServiceLocator { get; }
        protected IDictionary<string, ITestVariable> Variables { get; }
        private string method;

        public FormatTransformer() : this(null, null) { }
        public FormatTransformer(ServiceLocator serviceLocator, IDictionary<string, ITestVariable> variables)
            => (ServiceLocator, Variables) = (serviceLocator, variables);

        public void Initialize(string code)
        {
           method = "{0:" + code + "}";
        }

        public object Execute(object value)
        {
            if (method == null)
                throw new InvalidOperationException();

            var factory = new CasterFactory<T>();
            var caster = factory.Instantiate();
            var typedValue = caster.Execute(value);

            var transformedValue = String.Format(method, typedValue);

            return transformedValue;
        }
    }
}
