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
    abstract class AbstractRanking : BaseRankingFilter
    {
        public AbstractRanking(string operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(1, operand, columnType, aliases, expressions) { }

        public AbstractRanking(int count, string operand, ColumnType columnType, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : base(aliases, expressions, operand, columnType)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("The value of count must be strictly positive.");
            TableLength = count;
        }

        public int TableLength { get; private set; }

        public override ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            var filteredRs = new ResultSet.ResultSet();
            var dataTable = rs.Table.Clone();
            dataTable.Clear();
            filteredRs.Load(dataTable);

            foreach (DataRow row in rs.Rows)
            {
                if (filteredRs.Rows.Count < TableLength || RowCompare(filteredRs.Table.Rows[TableLength - 1], row))
                    InsertRow(filteredRs.Table, row, RowCompare);
            }

            return filteredRs;
        }

        protected void InsertRow(DataTable table, DataRow newRow, Func<DataRow, DataRow, bool> rowCompare)
        {
            var i = 0;
            var isRowAdded = false;
            while (!isRowAdded)
            {
                if (table.Rows.Count == i || RowCompare(table.Rows[i], newRow))
                {
                    var row = table.NewRow();
                    row.ItemArray = newRow.ItemArray.Clone() as object[];
                    table.Rows.InsertAt(row, i);
                    isRowAdded = true;
                }
                i++;
            }

            if (table.Rows.Count > TableLength)
                table.Rows.RemoveAt(TableLength);
        }

        protected virtual bool RowCompare(DataRow rowX, DataRow rowY)
        {
            var info = new PredicateInfo();
            var factory = new PredicateFilterFactory();
            var reference = GetValueFromRow(rowX, operand);
            var predicateInfo = BuildPredicateInfo(reference);
            var predicate = factory.Instantiate(aliases, expressions, predicateInfo);
            return predicate.Execute(rowY);
        }

        protected IPredicateInfo BuildPredicateInfo(object reference)
            => new PredicateInfo()
            {
                Operand = operand,
                ColumnType = columnType,
                ComparerType = GetComparerType(),
                Reference = reference
            };

        protected abstract ComparerType GetComparerType(); 

        protected class PredicateInfo : IPredicateInfo, IReferencePredicateInfo
        {
            public string Operand { get; set; }
            public ColumnType ColumnType { get; set; }
            public ComparerType ComparerType { get; set; }
            public bool Not { get; set; }
            public object Reference { get; set; }
        }
    }
}
