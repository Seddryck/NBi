using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver.Query
{
    public abstract class  QueryResolverArgs
    {
        private readonly string connectionString;
        private readonly IEnumerable<IQueryParameter> parameters;
        private readonly IEnumerable<IQueryTemplateVariable> variables;
        private readonly int timeout;

        public string ConnectionString { get => connectionString; }
        public IEnumerable<IQueryParameter> Parameters { get => parameters; }
        public IEnumerable<IQueryTemplateVariable> Variables { get => variables; }
        public int Timeout { get => timeout; }

        public QueryResolverArgs(string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, int timeout)
        {
            this.connectionString = connectionString;
            this.parameters = parameters;
            this.variables = variables;
            this.timeout = timeout;
        }
    }
}
