using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Extensibility;

namespace NBi.Core.Calculation.Ranking
{
    class TopRanking : AbstractRanking
    {
        public TopRanking(IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(1, operand, columnType, aliases, expressions) { }

        public TopRanking(int count, IColumnIdentifier operand, ColumnType columnType)
            : this(count, operand, columnType, null, null) { }

        public TopRanking(int count, IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : base(count, operand, columnType, aliases, expressions) { }

        protected override ComparerType GetComparerType() => ComparerType.MoreThan; 

        public override string Describe()
            => TableLength == 1
            ? "The first row of the result-set."
            : $"The first {TableLength} rows of the result-set";
    }
}
