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
            switch (args)
            {
                case UnionArgs x:
                    switch (x.Identity)
                    {
                        case ColumnIdentity.Ordinal: return new UnionByOrdinalEngine(x.ResultSetResolver);
                        case ColumnIdentity.Name: return new UnionByNameEngine(x.ResultSetResolver);
                        default: throw new NotImplementedException();
                    }
                case CartesianProductArgs x: return new CartesianProductEngine(x);
                default: throw new ArgumentException();
            }
        }
    }
}
