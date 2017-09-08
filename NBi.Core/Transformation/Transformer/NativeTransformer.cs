using Microsoft.CSharp;
using NBi.Core.ResultSet.Converter;
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
        private INativeTransformation transformation;

        public NativeTransformer()
        {
        }

        public void Initialize(string code)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var className = textInfo.ToTitleCase(code.Trim().Replace("-", " ")).Replace(" ", "");

            var clazz = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(
                                t => t.IsClass 
                                && t.IsAbstract==false
                                && t.Name == className 
                                && t.GetInterface("INativeTransformation")!=null)
                       .SingleOrDefault();

            if (clazz == null)
                throw new NotImplementedTransformationException(code);

            transformation = (INativeTransformation)Activator.CreateInstance(clazz);
        }

        public object Execute(object value)
        {
            if (transformation == null)
                throw new InvalidOperationException();

            var factory = new ConverterFactory<T>();
            var converter = factory.Build();
            var typedValue = converter.Convert(value);

            var transformedValue = transformation.Evaluate(typedValue);

            return transformedValue;
        }
    }
}
