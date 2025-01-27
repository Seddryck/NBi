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

class ReportDataSetQueryResolver : IQueryResolver
{
    private readonly ReportDataSetQueryResolverArgs args;
    private readonly ReportingParserFactory factory = new ReportingParserFactory();

    public ReportDataSetQueryResolver(ReportDataSetQueryResolverArgs args)
    { 
        this.args = args;
    }

    internal ReportDataSetQueryResolver(ReportDataSetQueryResolverArgs args, ReportingParserFactory factory)
        : this(args)
    {
        this.factory = factory;
    }

    public IQuery Execute()
    {
        var parser = factory.Instantiate(args.Source);

        var request = new ReportDataSetRequest(args.Source, args.Path,  args.Name, args.DataSetName);

        var reportParsingResult = parser.ExtractCommand(request);

        var query = new Query(reportParsingResult.Text, args.ConnectionString, args.Timeout, args.Parameters, args.Variables, reportParsingResult.CommandType);
        return query;
    }
}
