using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class MergingFactory
    {
        public IMergingEngine Instantiate(IMergingArgs args)
        {
            return args switch
            {
                UnionArgs x => x.Identity switch
                {
                    ColumnIdentity.Ordinal => new UnionByOrdinalEngine(x.ResultSetResolver),
                    ColumnIdentity.Name => new UnionByNameEngine(x.ResultSetResolver),
                    _ => throw new NotImplementedException(),
                },
                CartesianProductArgs x => new CartesianProductEngine(x),
                _ => throw new ArgumentException(),
            };
        }
    }
}
