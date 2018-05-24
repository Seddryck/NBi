using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking.Scoring;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public abstract class BaseRankingFilter : IResultSetFilter
    {
        protected readonly IColumnIdentifier operand;
        protected readonly ColumnType columnType;
        protected readonly IEnumerable<IColumnAlias> aliases;
        protected readonly IEnumerable<IColumnExpression> expressions;

        protected BaseRankingFilter(IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            this.operand = operand;
            this.columnType = columnType;
            this.aliases = aliases;
            this.expressions = expressions;
        }

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
            => throw new NotImplementedException();

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            IList<ScoredObject> subset = new List<ScoredObject>();
            var scorer = new DataRowScorer(operand, aliases, expressions);
            foreach (DataRow row in rs.Rows)
            {
                var score = scorer.Execute(row);
                InsertRow(score, ref subset);
            }

            var newRs = rs.Clone();
            newRs.Load(subset.Select(x => x.Value as DataRow));
            return newRs;
        }

        protected abstract void InsertRow(ScoredObject score, ref IList<ScoredObject> subset);
        

        public abstract string Describe();
    }
}
