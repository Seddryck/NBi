using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public class QueryResolverArgs : BaseQueryResolverArgs
{
    public string Statement { get; }
    public CommandType CommandType { get; }

    public QueryResolverArgs(string statement, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout, CommandType commandType)
        : base(connectionString, parameters, variables, timeout)
    {
        Statement = statement;
        CommandType = commandType;
    }
}
