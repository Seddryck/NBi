using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Ranking
{
    class BottomRanking : AbstractRanking
    {
        public BottomRanking(string operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(1, operand, columnType, aliases, expressions) { }

        public BottomRanking(int count, string operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : base(count, operand, columnType, aliases, expressions) { }

        protected override ComparerType GetComparerType() => ComparerType.LessThan; 

        public override string Describe()
            => TableLength == 1
            ? "The last row of the result-set."
            : $"The last {TableLength} rows of the result-set";
    }
}
