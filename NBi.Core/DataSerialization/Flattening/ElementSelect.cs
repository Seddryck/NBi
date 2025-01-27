using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening;

public class ElementSelect : IPathSelect
{
    public IScalarResolver<string> Path { get; }

    internal ElementSelect(IScalarResolver<string> path)
        => Path = path;
}
