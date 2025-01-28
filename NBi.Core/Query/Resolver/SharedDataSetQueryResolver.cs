using NBi.Core.Assemblies;
using NBi.Core.Query;
using NBi.Core.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

class SharedDataSetQueryResolver : IQueryResolver
{
    private readonly SharedDataSetQueryResolverArgs args;
    private readonly ReportingParserFactory factory = new ReportingParserFactory();

    public SharedDataSetQueryResolver(SharedDataSetQueryResolverArgs args)
    { 
        this.args = args;
    }

    internal SharedDataSetQueryResolver(SharedDataSetQueryResolverArgs args, ReportingParserFactory factory)
        : this(args)
    {
        this.factory = factory;
    }

    public IQuery Execute()
    {
        var parser = factory.Instantiate(args.Source);

        var request = new SharedDatasetRequest(args.Source, args.Path,  args.Name);

        var parsingResult = parser.ExtractCommand(request);

        var query = new Query(parsingResult.Text, args.ConnectionString, args.Timeout, args.Parameters, args.Variables, parsingResult.CommandType);
        return query;
    }
}
