using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
    class QueryResolverFactory
    {
        public IQueryResolver Instantiate(QueryResolverArgs args)
        {
            if (args is AssemblyQueryResolverArgs)
                return new AssemblyQueryResolver((AssemblyQueryResolverArgs)args);
            else if (args is ExternalFileQueryResolverArgs)
                return new ExternalFileQueryResolver((ExternalFileQueryResolverArgs)args);
            else if (args is EmbeddedQueryResolverArgs)
                return new EmbeddedQueryResolver((EmbeddedQueryResolverArgs)args);
            else if (args is DbCommandQueryResolverArgs)
                return new DbCommandQueryResolver((DbCommandQueryResolverArgs)args);

            throw new ArgumentException();
        }
    }
}
