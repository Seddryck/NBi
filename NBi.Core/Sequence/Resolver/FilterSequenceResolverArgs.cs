using NBi.Core.Assemblies;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class FilterSequenceResolverArgs : ISequenceResolverArgs
{
    public ISequenceResolver Resolver { get; }
    public ITransformer? OperandTransformation { get; }
    public IPredicate Predicate { get; }

    public FilterSequenceResolverArgs(ISequenceResolver resolver, IPredicate predicate, ITransformer? transformation)
        => (Resolver, Predicate, OperandTransformation) = (resolver, predicate, transformation);
}
