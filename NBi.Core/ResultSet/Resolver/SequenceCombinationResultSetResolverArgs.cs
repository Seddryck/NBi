using NBi.Core.Query;
using NBi.Core.Sequence.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class SequenceCombinationResultSetResolverArgs : ResultSetResolverArgs
{
    public IEnumerable<ISequenceResolver> Resolvers { get; }
    public SequenceCombinationResultSetResolverArgs(IEnumerable<ISequenceResolver> resolvers)
        => Resolvers = resolvers;
}
