using NBi.Core.Injection;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Scalar.Resolver
{
    public class ScalarResolverFactory
    {
        private readonly ServiceLocator serviceLocator;
        private readonly MethodInfo instantiateHandler;

        public ScalarResolverFactory(ServiceLocator? serviceLocator = null)
        {
            this.serviceLocator = serviceLocator ?? new();
            instantiateHandler = GetType().GetMethods().Single(x => x.Name == nameof(Instantiate) && x.IsGenericMethod);
        }

        public IScalarResolver Instantiate(IScalarResolverArgs args)
        {
            return args switch
            {
                FormatScalarResolverArgs _ => Instantiate<string>(args),
                _ => Instantiate<object>(args),
            };
        }

        public IScalarResolver Instantiate(IScalarResolverArgs args, Type type)
            => (instantiateHandler
                .MakeGenericMethod(type)
                .Invoke(this, new[] { args }))
                as IScalarResolver ?? throw new NotSupportedException();

        public IScalarResolver<T> Instantiate<T>(IScalarResolverArgs args)
        {
            return args switch
            {
                LiteralScalarResolverArgs x => new LiteralScalarResolver<T>(x),
                GlobalVariableScalarResolverArgs x => new GlobalVariableScalarResolver<T>(x),
                ContextScalarResolverArgs x => new ContextScalarResolver<T>(x),
                QueryScalarResolverArgs x => new QueryScalarResolver<T>(x, serviceLocator),
                ProjectionResultSetScalarResolverArgs x => new ProjectionResultSetScalarResolver<T>(x, serviceLocator.GetResultSetResolverFactory()),
                CSharpScalarResolverArgs x => new CSharpScalarResolver<T>(x),
                NCalcScalarResolverArgs x => new NCalcScalarResolver<T>(x),
                EnvironmentScalarResolverArgs x => new EnvironmentScalarResolver<T>(x),
                CustomScalarResolverArgs x => new CustomScalarResolver<T>(x),
                FormatScalarResolverArgs x => typeof(T) == typeof(string) ? (IScalarResolver<T>)new FormatScalarResolver(x, serviceLocator) : throw new ArgumentException("You cannot instantiate a FormatScalarResolver that is not a string."),
                FunctionScalarResolverArgs x => new FunctionScalarResolver<T>(x),
                _ => throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar"),
            };
        }
    }
}
