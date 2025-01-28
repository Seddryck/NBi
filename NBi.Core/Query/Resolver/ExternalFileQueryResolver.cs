using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility;

namespace NBi.Core.Query.Resolver;

class ExternalFileQueryResolver : IQueryResolver
{
    private readonly ExternalFileQueryResolverArgs args;
    
    public ExternalFileQueryResolver(ExternalFileQueryResolverArgs args)
    {
        this.args = args;
    }

    public IQuery Execute()
    {
        if (!System.IO.File.Exists(args.Path))
            throw new ExternalDependencyNotFoundException(args.Path);
        var commandText = System.IO.File.ReadAllText(args.Path, Encoding.UTF8);

        var query = new Query(commandText, args.ConnectionString, args.Timeout, args.Parameters, args.Variables);
        return query;
    }
}
