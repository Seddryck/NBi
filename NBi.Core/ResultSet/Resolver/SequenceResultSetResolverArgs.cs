using NBi.Core.Query;
using NBi.Core.Sequence.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class SequenceResultSetResolverArgs : SequenceCombinationResultSetResolverArgs
    {
        public SequenceResultSetResolverArgs(ISequenceResolver resolver)
            : base([resolver]) { }
    }
}
