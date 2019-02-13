using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;
using NBi.Core.Injection;

namespace NBi.Core.Scalar.Resolver
{
    public class ScalarResolverFactory
    {
        private readonly ServiceLocator serviceLocator;
        public ScalarResolverFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IScalarResolver<T> Instantiate<T>(IScalarResolverArgs args)
        {
            switch (args)
            {
                case LiteralScalarResolverArgs x:
                    return new LiteralScalarResolver<T>(x);
                case GlobalVariableScalarResolverArgs x:
                    return new GlobalVariableScalarResolver<T>(x);
                case QueryScalarResolverArgs x:
                    return new QueryScalarResolver<T>(x, serviceLocator);
                case ProjectionResultSetScalarResolverArgs x:
                    return new ProjectionResultSetScalarResolver<T>(x, serviceLocator.GetResultSetResolverFactory());
                case CSharpScalarResolverArgs x:
                    return new CSharpScalarResolver<T>(x);
                case EnvironmentScalarResolverArgs x:
                    return new EnvironmentScalarResolver<T>(x);
                case FormatScalarResolverArgs x:
                    return (IScalarResolver<T>)new FormatScalarResolver(x, serviceLocator);
                case FunctionScalarResolverArgs x:
                    return new FunctionScalarResolver<T>(x);
                default:
                    throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
            }
        }
    }
}
