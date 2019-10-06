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
    class NCalcTransformer<T> : ITransformer
    {
        private ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }
        private NCalc.Expression method;

        public NCalcTransformer() : this(null, null) { }
        public NCalcTransformer(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public void Initialize(string code)
        {
           method = new NCalc.Expression(code);
        }

        public object Execute(object value)
        {
            if (method == null)
                throw new InvalidOperationException();

            if (method.Parameters.ContainsKey("value"))
                method.Parameters["value"] = value;
            else
                method.Parameters.Add("value", value);

            var transformedValue = method.Evaluate();

            return transformedValue;
        }
    }
}
