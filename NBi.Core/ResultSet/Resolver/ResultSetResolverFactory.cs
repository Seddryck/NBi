using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;

namespace NBi.Core.ResultSet.Resolver
{
    public class ResultSetResolverFactory
    {
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;

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
                return new QueryResultSetResolver(args as QueryResultSetResolverArgs);
            else if (args is CsvResultSetResolverArgs)
                return new CsvResultSetResolver(args as CsvResultSetResolverArgs);
            else if (args is XPathResultSetResolverArgs)
                return new XPathResultSetResolver(args as XPathResultSetResolverArgs);
            else if (args is ObjectsResultSetResolverArgs)
                return new ObjectsResultSetResolver(args as ObjectsResultSetResolverArgs);

            throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a ResultSet");
        }
    }
}
