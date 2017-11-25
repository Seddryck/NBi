using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;

namespace NBi.Core.Scalar.Resolver
{
    public class ScalarResolverFactory
    {
        public IScalarResolver<T> Instantiate<T>(IScalarResolverArgs args)
        {
            if (args is LiteralScalarResolverArgs)
                    return new LiteralScalarResolver<T>((LiteralScalarResolverArgs)args); 
            else if (args is GlobalVariableScalarResolverArgs)
                return new GlobalVariableScalarResolver<T>((GlobalVariableScalarResolverArgs)args);
            else if (args is QueryScalarResolverArgs)
                return new QueryScalarResolver<T>((QueryScalarResolverArgs)args);
            else if (args is ProjectionResultSetScalarResolverArgs)
                return new ProjectionResultSetScalarResolver<T>((ProjectionResultSetScalarResolverArgs)args);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Scalar");
        }
    }
}
