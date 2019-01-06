using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;
using NBi.Core.Injection;
using NBi.Core.FlatFile;

namespace NBi.Core.ResultSet.Resolver
{
    public class ResultSetResolverFactory
    {
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;
        private readonly ServiceLocator serviceLocator;
        
        public ResultSetResolverFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Using(CsvProfile profile)
        {
            if (profile != null)
                this.profile = profile;
        }

        public IResultSetResolver Instantiate(ResultSetResolverArgs args)
        {
            if (args is ContentResultSetResolverArgs)
                    return new ContentResultSetResolver(args as ContentResultSetResolverArgs); 
            else if (args is RowsResultSetResolverArgs)
                return new RowsResultSetResolver(args as RowsResultSetResolverArgs);
            else if (args is QueryResultSetResolverArgs)
                return new QueryResultSetResolver(args as QueryResultSetResolverArgs, serviceLocator);
            else if (args is FlatFileResultSetResolverArgs)
                return new FlatFileResultSetResolver(args as FlatFileResultSetResolverArgs);
            else if (args is XPathResultSetResolverArgs)
                return new XPathResultSetResolver(args as XPathResultSetResolverArgs);
            else if (args is ObjectsResultSetResolverArgs)
                return new ObjectsResultSetResolver(args as ObjectsResultSetResolverArgs);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a ResultSet");
        }
    }
}
