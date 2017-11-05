using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Resolver
{
    class EmbeddedQueryResolver : IQueryResolver
    {
        private readonly EmbeddedQueryResolverArgs args;

        public EmbeddedQueryResolver(EmbeddedQueryResolverArgs args)
        {
            this.args = args;
        }

        public IDbCommand Execute()
        {
            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(args.ConnectionString, args.CommandText, args.Parameters, args.Variables, args.Timeout);

            return cmd;
        }
    }
}
