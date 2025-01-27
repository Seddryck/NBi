using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;

namespace NBi.Core.ResultSet.Resolver;

class IfUnavailableResultSetResolver : IResultSetResolver
{
    private IfUnavailableResultSetResolverArgs Args { get; }

    public IfUnavailableResultSetResolver(IfUnavailableResultSetResolverArgs args)
        => Args = args;
    
    public IResultSet Execute()
    {
        try
        { return Args.Primary.Execute(); }
        catch (ResultSetUnavailableException)
        { return Args.Secondary.Execute(); }
    }
}
