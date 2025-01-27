using NBi.Core.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Resolver;

public class QueryResolverFactory
{
    private readonly ServiceLocator serviceLocator;

    public QueryResolverFactory(ServiceLocator serviceLocator)
    {
        this.serviceLocator = serviceLocator;
    }

    public virtual IQueryResolver Instantiate(BaseQueryResolverArgs args)
    {
        if (args is AssemblyQueryResolverArgs)
            return new AssemblyQueryResolver((AssemblyQueryResolverArgs)args);
        else if (args is ExternalFileQueryResolverArgs)
            return new ExternalFileQueryResolver((ExternalFileQueryResolverArgs)args);
        else if (args is EmbeddedQueryResolverArgs)
            return new EmbeddedQueryResolver((EmbeddedQueryResolverArgs)args);
        else if (args is ReportDataSetQueryResolverArgs)
            return new ReportDataSetQueryResolver((ReportDataSetQueryResolverArgs)args);
        else if (args is SharedDataSetQueryResolverArgs)
            return new SharedDataSetQueryResolver((SharedDataSetQueryResolverArgs)args);
        else if (args is QueryResolverArgs)
            return new QueryResolver((QueryResolverArgs)args);

        throw new ArgumentException();
    }
}
