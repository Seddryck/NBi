using NBi.Core.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure.Olap.PostFilters;

class DimensionType : IPostCommandFilter
{
    public bool Evaluate(object row)
    {
        if (row is IDimensionType)
            return Evaluate((IDimensionType)row);

        throw new ArgumentException();
    }

    protected bool Evaluate(IDimensionType row)
    {
        return !row.DimensionType.Equals(2);
    }
}
