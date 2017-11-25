using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Resolver
{
    class DbCommandQueryResolver : IQueryResolver
    {
        private readonly DbCommandQueryResolverArgs args;

        public DbCommandQueryResolver(DbCommandQueryResolverArgs args)
        {
            this.args = args;
        }

        public IDbCommand Execute()
        {
            return args.Command;
        }
    }
}
