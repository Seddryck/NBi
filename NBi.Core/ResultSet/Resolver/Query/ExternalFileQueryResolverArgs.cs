using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
    public class ExternalFileQueryResolverArgs : QueryResolverArgs
    {
        private readonly string path;

        public string Path { get => path; }

        public ExternalFileQueryResolverArgs(string path, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
            : base(connectionString, parameters, variables, timeout)
        {
            this.path = path;
        }
    }
}
