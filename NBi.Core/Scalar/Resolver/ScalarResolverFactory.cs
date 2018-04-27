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
            if (args is LiteralScalarResolverArgs)
                    return new LiteralScalarResolver<T>((LiteralScalarResolverArgs)args); 
            else if (args is GlobalVariableScalarResolverArgs)
                return new GlobalVariableScalarResolver<T>((GlobalVariableScalarResolverArgs)args);
            else if (args is QueryScalarResolverArgs)
                return new QueryScalarResolver<T>((QueryScalarResolverArgs)args, serviceLocator);
            else if (args is ProjectionResultSetScalarResolverArgs)
                return new ProjectionResultSetScalarResolver<T>((ProjectionResultSetScalarResolverArgs)args, serviceLocator.GetResultSetResolverFactory());
            else if (args is CSharpScalarResolverArgs)
                return new CSharpScalarResolver<T>((CSharpScalarResolverArgs)args);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
        }
    }
}
