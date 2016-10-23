using Microsoft.CSharp;
using NBi.Core.ResultSet.Converter;
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

            //var factory = new ConverterFactory<T>();
            //var converter = factory.Build();
            //var typedValue = converter.Convert(value);

            if (method.Parameters.ContainsKey("value"))
                method.Parameters["value"] = value;
            else
                method.Parameters.Add("value", value);

            var transformedValue = method.Evaluate();

            return transformedValue;
        }
    }
}
