using NBi.Extensibility.Resolving;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class ListSequenceResolverArgs : ISequenceResolverArgs
{
    public IEnumerable<IScalarResolver> Resolvers { get; }

    public ListSequenceResolverArgs(IEnumerable<IScalarResolver> resolvers)
    {
        Resolvers = resolvers;
    }
}
