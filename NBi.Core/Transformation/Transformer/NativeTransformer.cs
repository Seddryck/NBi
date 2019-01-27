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
        private INativeTransformation transformation;

        public NativeTransformer()
        {
        }

        public void Initialize(string code)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var parameters = code.Replace("(", ",")
                .Replace(")", ",")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().Skip(1).Select(x => x.Trim()).ToArray();

            var classToken = code.Contains("(") ? code.Replace(" ","").Substring(0, code.IndexOf('(')) : code;
            var className = textInfo.ToTitleCase(classToken.Trim().Replace("-", " ")).Replace(" ", "").Replace("Datetime", "DateTime");

            var clazz = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(
                                t => t.IsClass
                                && t.IsAbstract == false
                                && t.Name == className
                                && t.GetInterface("INativeTransformation") != null)
                       .SingleOrDefault();

            if (clazz == null)
                throw new NotImplementedTransformationException(className);

            transformation = (INativeTransformation)Activator.CreateInstance(clazz, parameters);
        }

        public object Execute(object value)
        {
            if (transformation == null)
                throw new InvalidOperationException();

            var factory = new CasterFactory<T>();
            var caster = factory.Instantiate();

            object typedValue = null;

            if (value == null || value == DBNull.Value || value as string == "(null)")
                typedValue = null;
            else if ((typeof(T)!=typeof(string)) && (value is string) && ((string.IsNullOrEmpty(value as string) || value as string == "(empty)")))
                typedValue = null;
            else
                typedValue = (object)(caster.Execute(value));

            var transformedValue = transformation.Evaluate(typedValue);

            return transformedValue;
        }
    }
}
