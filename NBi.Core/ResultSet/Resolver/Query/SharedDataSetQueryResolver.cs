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

        public IDbCommand Execute()
        {
            var parser = factory.Instantiate(args.Source);

            var request = new SharedDatasetRequest(args.Source, args.Path,  args.Name);

            var parsingResult = parser.ExtractCommand(request);

            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(args.ConnectionString, parsingResult.Text, parsingResult.CommandType, args.Parameters, args.Variables, args.Timeout);

            return cmd;
        }
    }
}
