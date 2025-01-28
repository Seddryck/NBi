using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public class EnvironmentScalarResolverArgs : IScalarResolverArgs
{
    public string Name { get; }

    public EnvironmentScalarResolverArgs(string name)
    {
        Name = name;
    }
}
