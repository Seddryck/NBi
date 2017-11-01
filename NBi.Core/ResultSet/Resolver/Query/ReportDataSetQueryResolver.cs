using NBi.Core.Assemblies;
using NBi.Core.Query;
using NBi.Core.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
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

        public IDbCommand Execute()
        {
            var parser = factory.Instantiate(args.Source);

            var request = new ReportDataSetRequest(args.Source, args.Path,  args.Name, args.DataSetName);

            var reportParsingResult = parser.ExtractCommand(request);

            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(args.ConnectionString, reportParsingResult.Text, reportParsingResult.CommandType, args.Parameters, args.Variables, args.Timeout);

            return cmd;
        }
    }
}
