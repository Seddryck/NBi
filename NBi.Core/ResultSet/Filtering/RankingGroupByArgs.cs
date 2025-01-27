using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Ranking;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

public class RankingGroupByArgs : RankingArgs, IFilteringArgs
{
    public IGroupBy GroupBy { get; }

    public RankingGroupByArgs(IGroupBy groupBy, RankingOption option, int count, IColumnIdentifier operand, ColumnType type)
        : base (option, count, operand, type) => GroupBy = groupBy;
}
