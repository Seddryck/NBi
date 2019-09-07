using Microsoft.CSharp;
using NBi.Core.Scalar.Casting;
using NBi.Core.Transformation.Transformer.Native;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    class NativeTransformer<T> : ITransformer
    {
        private IList<INativeTransformation> Transformations { get; } = new List<INativeTransformation>();
        private bool IsInitialized { get; set; } = false;

        public NativeTransformer()
        { }

        public void Initialize(string code)
        {
            var functions = code.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var function in functions)
            {
                var transformation = new NativeTransformationFactory().Instantiate(function);
                Transformations.Add(transformation);
            }
            IsInitialized = true;
        }

        public object Execute(object value)
        {
            if (!IsInitialized)
                throw new InvalidOperationException();

            var factory = new CasterFactory<T>();
            var caster = factory.Instantiate();

            object typedValue;
            if (value == null || value == DBNull.Value || value as string == "(null)")
                typedValue = null;
            else if ((typeof(T)!=typeof(string)) && (value is string) && ((string.IsNullOrEmpty(value as string) || value as string == "(empty)")))
                typedValue = null;
            else
                typedValue = caster.Execute(value);

            object transformedValue = typedValue;

            foreach (var transformation in Transformations)
                transformedValue = transformation.Evaluate(transformedValue);

            return transformedValue;
        }
    }
}
