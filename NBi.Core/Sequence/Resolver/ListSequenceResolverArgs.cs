using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class ListSequenceResolverArgs : ISequenceResolverArgs
    {
        public IEnumerable<object> Objects { get; }

        public ListSequenceResolverArgs(IEnumerable<object> objects)
        {
            this.Objects = objects;
        }
    }
}
