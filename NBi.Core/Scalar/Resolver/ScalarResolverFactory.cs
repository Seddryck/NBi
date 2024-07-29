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
            switch (args)
            {
                case FormatScalarResolverArgs _: return Instantiate<string>(args);
                default: return Instantiate<object>(args);
            }
        }

        public IScalarResolver Instantiate(IScalarResolverArgs args, Type type)
            => (instantiateHandler
                .MakeGenericMethod(type)
                .Invoke(this, new[] { args }))
                as IScalarResolver ?? throw new NotSupportedException();

        public IScalarResolver<T> Instantiate<T>(IScalarResolverArgs args)
        {
            switch (args)
            {
                case LiteralScalarResolverArgs x:
                    return new LiteralScalarResolver<T>(x);
                case GlobalVariableScalarResolverArgs x:
                    return new GlobalVariableScalarResolver<T>(x);
                case ContextScalarResolverArgs x:
                    return new ContextScalarResolver<T>(x);
                case QueryScalarResolverArgs x:
                    return new QueryScalarResolver<T>(x, serviceLocator);
                case ProjectionResultSetScalarResolverArgs x:
                    return new ProjectionResultSetScalarResolver<T>(x, serviceLocator.GetResultSetResolverFactory());
                case CSharpScalarResolverArgs x:
                    return new CSharpScalarResolver<T>(x);
                case NCalcScalarResolverArgs x:
                    return new NCalcScalarResolver<T>(x);
                case EnvironmentScalarResolverArgs x:
                    return new EnvironmentScalarResolver<T>(x);
                case CustomScalarResolverArgs x:
                    return new CustomScalarResolver<T>(x);
                case FormatScalarResolverArgs x:
                    return typeof(T) == typeof(string) ? (IScalarResolver<T>)new FormatScalarResolver(x, serviceLocator) : throw new ArgumentException("You cannot instantiate a FormatScalarResolver that is not a string.");
                case FunctionScalarResolverArgs x:
                    return new FunctionScalarResolver<T>(x);
                default:
                    throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
            }
        }
    }
}
