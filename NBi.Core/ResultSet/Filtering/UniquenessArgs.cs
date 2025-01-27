using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Ranking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

public class UniquenessArgs : IFilteringArgs
{
    public IGroupBy GroupBy { get; }

    public UniquenessArgs(IGroupBy groupBy)
        => GroupBy = groupBy;
}
