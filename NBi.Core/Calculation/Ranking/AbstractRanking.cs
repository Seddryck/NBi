using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Ranking.Scoring;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Ranking
{
    public abstract class AbstractRanking : BaseRankingFilter
    {
        public AbstractRanking(IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(1, operand, columnType, aliases, expressions) { }

        public AbstractRanking(int count, IColumnIdentifier operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : base(operand, columnType, aliases, expressions)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("The value of count must be strictly positive.");
            TableLength = count;
        }

        public int TableLength { get; private set; }

        
        protected override void InsertRow(ScoredObject newObj, ref IList<ScoredObject> list)
        {
            var i = 0;
            var isObjAdded = false;
            while (!isObjAdded)
            {
                if (list.Count == i || RowCompare(list[i], newObj))
                {
                    list.Insert(i, newObj);
                    isObjAdded = true;
                }
                i++;
            }

            if (list.Count > TableLength)
                list.RemoveAt(TableLength);
        }

        protected virtual bool RowCompare(ScoredObject oldObj, ScoredObject newObj)
        {
            var info = new PredicateInfo();
            var factory = new PredicateFactory();
            var predicateInfo = BuildPredicateInfo(oldObj.Score);
            var predicate = factory.Instantiate(predicateInfo);
            return predicate.Execute(newObj.Score);
        }

        

        private IPredicateInfo BuildPredicateInfo(object reference)
            => new PredicateInfo()
            {
                Operand = operand,
                ColumnType = columnType,
                ComparerType = GetComparerType(),
                Reference = reference
            };

        protected abstract ComparerType GetComparerType();

        private class PredicateInfo : IPredicateInfo, IReferencePredicateInfo
        {
            public IColumnIdentifier Operand { get; set; }
            public ColumnType ColumnType { get; set; }
            public ComparerType ComparerType { get; set; }
            public bool Not { get; set; }
            public object Reference { get; set; }
        }
    }
}
