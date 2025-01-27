using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public class EmbeddedQueryResolverArgs : BaseQueryResolverArgs
{
    private readonly string commandText;

    public string CommandText { get => commandText; }

    public EmbeddedQueryResolverArgs(string commandText, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
        : base(connectionString, parameters, variables, timeout)
    {
        this.commandText = commandText;
    }
}
