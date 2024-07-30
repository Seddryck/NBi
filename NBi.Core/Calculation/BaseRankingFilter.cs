using NBi.Core.Calculation.Ranking.Scoring;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;
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
        protected Func<IResultSet, IResultSet> Execution { get; }

        protected BaseRankingFilter(IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            Operand = operand;
            ColumnType = columnType;
            Aliases = aliases;
            Expressions = expressions;
            Execution = Apply;
        }

        public IResultSet Execute(IResultSet rs)
            => Execution.Invoke(rs);    

        public IResultSet AntiApply(IResultSet rs)
            => throw new NotImplementedException();

        public IResultSet Apply(IResultSet rs)
        {
            IList<ScoredObject> subset = [];
            var scorer = new DataRowScorer(Operand, Aliases, Expressions);
            foreach (var row in rs.Rows)
            {
                var score = scorer.Execute(row);
                InsertRow(score, ref subset);
            }

            var newRs = rs.Clone();
            newRs.AddRange(subset.Select(x => x.Value as IResultRow ?? throw new InvalidOperationException()));
            return newRs;
        }

        protected abstract void InsertRow(ScoredObject score, ref IList<ScoredObject> subset);

        public abstract string Describe();
    }
}
