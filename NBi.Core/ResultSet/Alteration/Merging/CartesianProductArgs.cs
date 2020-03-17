using NBi.Core.ResultSet.Resolver;
using NBi.Core.Sequence.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class CartesianProductArgs : IMergingArgs
    {
        public IResultSetService ResultSetResolver { get; }

        public CartesianProductArgs(IResultSetService resultSetResolver)
            => (ResultSetResolver) = (resultSetResolver);
    }
}
