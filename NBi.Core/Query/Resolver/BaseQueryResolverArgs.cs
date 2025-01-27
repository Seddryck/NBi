using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public abstract class BaseQueryResolverArgs
{
    public string ConnectionString { get; }
    public IEnumerable<IQueryParameter> Parameters { get; }
    public IEnumerable<IQueryTemplateVariable> Variables { get; }
    public TimeSpan Timeout { get; }

    public BaseQueryResolverArgs(string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
    {
        ConnectionString = connectionString;
        Parameters = parameters;
        Variables = variables;
        Timeout = timeout;
    }
}
