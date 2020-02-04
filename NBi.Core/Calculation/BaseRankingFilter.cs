using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking.Scoring;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Core.Variable;
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
        protected IColumnIdentifier Operand { get; }
        protected ColumnType ColumnType { get; }
        protected IEnumerable<IColumnAlias> Aliases { get; }
        protected IEnumerable<IColumnExpression> Expressions { get; }

        protected BaseRankingFilter(IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            this.Operand = operand;
            this.ColumnType = columnType;
            this.Aliases = aliases;
            this.Expressions = expressions;
        }

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
            => throw new NotImplementedException();

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            IList<ScoredObject> subset = new List<ScoredObject>();
            var scorer = new DataRowScorer(Operand, Aliases, Expressions);
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
