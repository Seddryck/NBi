using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking
{
    public interface IRankingInfo
    {
        RankingOption Option { get; set; }
        int Count { get; set; }
        IColumnIdentifier Operand { get; set; }
        ColumnType ColumnType { get; set; }
        IEnumerable<IColumnAlias> Aliases { get; set; }
        IEnumerable<IColumnExpression> Expressions { get; set; }
    }
}
