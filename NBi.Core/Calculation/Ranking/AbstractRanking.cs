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
using NBi.Core.Scalar.Resolver;

namespace NBi.Core.Calculation.Ranking
{
    public abstract class AbstractRanking : BaseRankingFilter
    {
        protected abstract ComparerType GetComparerType();

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
            switch (columnType)
            {
                case ColumnType.Text: return RowCompare<string>(oldObj, newObj);
                case ColumnType.Numeric: return RowCompare<decimal>(oldObj, newObj);
                case ColumnType.DateTime: return RowCompare<DateTime>(oldObj, newObj);
                case ColumnType.Boolean: return RowCompare<bool>(oldObj, newObj);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual bool RowCompare<T>(ScoredObject oldObj, ScoredObject newObj)
        {
            var factory = new PredicateFactory();
            var predicateArgs = new ReferencePredicateArgs()
            {
                ColumnType = columnType,
                ComparerType = GetComparerType(),
                Reference = new LiteralScalarResolver<T>(oldObj.Score)
            };
            var predicate = factory.Instantiate(predicateArgs);
            return predicate.Execute(newObj.Score);
        }
    }
}
