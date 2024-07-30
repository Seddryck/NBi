using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class CartesianProductEngine : IMergingEngine
    {
        private CartesianProductArgs Args { get; }

        public CartesianProductEngine(CartesianProductArgs args)
            => Args = args;

        public IResultSet Execute(IResultSet rs)
        {
            var secondRs = Args.ResultSetResolver.Execute();

            var initialColumnCount = rs.ColumnCount;
            foreach (var dataColumn in secondRs.Columns)
            {
                while (rs.ContainsColumn(dataColumn.Name))
                    rs.GetColumn(dataColumn.Name)?.Rename($"{dataColumn.Name}_1");

                rs.AddColumn(dataColumn.Name, dataColumn.DataType);
            }

            if (secondRs.RowCount == 0 || secondRs.ColumnCount == 0)
            {
                rs.Clear();
            }
            else
            {
                var firstItem = secondRs[0];
                foreach (var row in rs.Rows)
                    foreach (var column in secondRs.Columns)
                        row[initialColumnCount + column.Ordinal] = firstItem[column.Ordinal];

                var newRows = new HashSet<IResultRow>();
                foreach (var item in secondRs.Rows.Skip(1))
                {
                    foreach (var row in rs.Rows)
                    {
                        var newRow = rs.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        foreach (var column in secondRs.Columns)
                            newRow[initialColumnCount + column.Ordinal] = item[column.Ordinal];
                        newRows.Add(newRow);
                    }
                }
                foreach (var newRow in newRows)
                    rs.AddRow(newRow);
            }
            rs.AcceptChanges();
            return rs;
        }
    }
}
