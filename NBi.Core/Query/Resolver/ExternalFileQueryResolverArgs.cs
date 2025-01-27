using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public class ExternalFileQueryResolverArgs : BaseQueryResolverArgs
{
    private readonly string path;

    public string Path { get => path; }

    public ExternalFileQueryResolverArgs(string path, string connectionString, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
        : base(connectionString, parameters, variables, timeout)
    {
        this.path = path;
    }
}
