using NBi.Core.Calculation.Asserting;
using NBi.Core.Calculation.Ranking.Scoring;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;

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
            return ColumnType switch
            {
                ColumnType.Text => RowCompare<string>(oldObj, newObj),
                ColumnType.Numeric => RowCompare<decimal>(oldObj, newObj),
                ColumnType.DateTime => RowCompare<DateTime>(oldObj, newObj),
                ColumnType.Boolean => RowCompare<bool>(oldObj, newObj),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        protected virtual bool RowCompare<T>(ScoredObject oldObj, ScoredObject newObj)
        {
            var factory = new PredicateFactory();
            var predicateArgs = new ReferencePredicateArgs(new LiteralScalarResolver<T>(oldObj.Score))
            {
                ColumnType = ColumnType,
                ComparerType = GetComparerType(),
            };
            var predicate = factory.Instantiate(predicateArgs);
            return predicate.Execute(newObj.Score);
        }
    }
}
