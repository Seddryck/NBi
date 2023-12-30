using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Transformation.Transformer.Native.IO;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    public class NativeTransformationFactory
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }
        public NativeTransformationFactory(ServiceLocator serviceLocator, Context context)
            => (ServiceLocator, Context) = (serviceLocator, context);

        public INativeTransformation Instantiate(string code)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var functionParameters = code.Replace("(", ",")
                .Replace(")", ",").Trim()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().Skip(1).Select(x => x.Trim()).ToList();

            var classToken = code.Contains("(") ? code.Substring(0, code.IndexOf('(')).Replace(" ", "") : code;
            var className = textInfo.ToTitleCase(classToken.Trim().Replace("-", " ")).Replace(" ", "").Replace("Datetime", "DateTime").Replace("Timespan", "TimeSpan");

            var type = typeof(INativeTransformation).Assembly.GetTypes()
                       .Where(
                                t => t.IsClass
                                && t.IsAbstract == false
                                && t.Name == className
                                && t.GetInterface(typeof(INativeTransformation).Name) != null)
                       .SingleOrDefault() ?? throw new NotImplementedTransformationException(className);
            var ctor = type.GetConstructors().SingleOrDefault(x => x.GetParameters().Count() == functionParameters.Count) ?? throw new MissingOrUnexpectedParametersTransformationException(className, functionParameters.Count());
            var zip = ctor.GetParameters().Zip(functionParameters, (x, y) => new { x.ParameterType, Value = y });
            var typedFunctionParameters = new List<object>();
            var argsFactory = new ScalarResolverArgsFactory(ServiceLocator, Context);
            var factory = ServiceLocator.GetScalarResolverFactory();

            foreach (var param in zip)
            {
                if (typeof(IScalarResolver).IsAssignableFrom(param.ParameterType))
                {
                    
                    var scalarType = param.ParameterType.GenericTypeArguments[0];
                    var args = argsFactory.Instantiate(param.Value);
                    var resolver = factory.Instantiate(args, scalarType);
                    typedFunctionParameters.Add(resolver);
                }
                else
                    typedFunctionParameters.Add(param.Value);
            }

            return (INativeTransformation)ctor.Invoke(typedFunctionParameters.ToArray());
        }
    }
}
