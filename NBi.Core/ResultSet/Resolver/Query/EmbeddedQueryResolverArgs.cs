using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
    public class EmbeddedQueryResolverArgs : QueryResolverArgs
    {
        private readonly string commandText;

        public string CommandText { get => commandText; }

        public EmbeddedQueryResolverArgs(string commandText, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
            : base(connectionString, parameters, variables, timeout)
        {
            this.commandText = commandText;
        }
    }
}
