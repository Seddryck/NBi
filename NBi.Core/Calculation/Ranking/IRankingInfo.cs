using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking;

public interface IRankingInfo
{
    RankingOption Option { get; }
    int Count { get; }
    IColumnIdentifier Operand { get; set; }
    ColumnType Type { get; set; }
}
