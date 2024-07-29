using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation.Transformer.Native;
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
            var expression = new Expressif.Expression(code, Context);
            return new NativeTransformation(expression);
        }
    }
}
