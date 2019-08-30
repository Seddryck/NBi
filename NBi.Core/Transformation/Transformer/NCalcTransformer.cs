using Microsoft.CSharp;
using NBi.Core.Scalar.Casting;
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
        private NCalc.Expression method;

        public NCalcTransformer()
        {
        }

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
