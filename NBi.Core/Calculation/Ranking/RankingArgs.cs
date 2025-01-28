using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking;

public class RankingArgs : IRankingInfo 
{
    public RankingOption Option { get; }

    public int Count { get; }

    public IColumnIdentifier Operand { get; set; }
    public ColumnType Type { get; set; }

    public RankingArgs(RankingOption option, int count, IColumnIdentifier operand, ColumnType type)
        => (Option, Count, Operand, Type) = (option, count, operand, type);
}
