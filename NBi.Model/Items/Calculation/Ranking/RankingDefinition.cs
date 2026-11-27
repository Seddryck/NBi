using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Model.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation.Ranking;

public class RankingDefinition : IRankingInfo
{

    public IColumnIdentifier Operand { get; set; }

    public ColumnType Type { get; set; }

    public BaseRank Rank { get; set; }

    public RankingOption Option
    {
        get => Rank.Option;
    }

    public int Count
    {
        get => Rank.Count;
    }

    public GroupBy GroupBy { get; set; }

    public RankingDefinition()
    {
        Type = ColumnType.Numeric;
    }
}
