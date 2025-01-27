using NBi.Extensibility.Resolving;
using NBi.Core.Transformation.Transformer.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public class FunctionScalarResolverArgs : IScalarResolverArgs
{
    public IScalarResolver Resolver { get; }
    public IEnumerable<INativeTransformation> Transformations { get; }

    public FunctionScalarResolverArgs(IScalarResolver resolver, IEnumerable<INativeTransformation> transformations)
    {
        Resolver = resolver;
        Transformations = transformations;
    }
}
