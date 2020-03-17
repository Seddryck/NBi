using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class CartesianProductEngine : IMergingEngine
    {
        private CartesianProductArgs Args { get; }

        public CartesianProductEngine(CartesianProductArgs args)
            => Args = args;

        public ResultSet Execute(ResultSet rs)
        {
            var secondRs = Args.ResultSetResolver.Execute();

            var initialColumnCount = rs.Columns.Count;
            foreach (DataColumn dataColumn in secondRs.Columns)
            {
                while (rs.Columns.Contains(dataColumn.ColumnName))
                    dataColumn.ColumnName = $"{dataColumn.ColumnName}_1";

                var newColumn = new DataColumn(dataColumn.ColumnName, dataColumn.DataType);
                rs.Columns.Add(newColumn);
            }

            if (secondRs.Rows.Count == 0 || secondRs.Columns.Count == 0)
            {
                rs.Table.Clear();
            }
            else
            {
                var firstItem = secondRs.Rows[0];
                foreach (DataRow row in rs.Rows)
                    foreach (DataColumn column in secondRs.Columns)
                        row[initialColumnCount + column.Ordinal] = firstItem[column.Ordinal];

                var newRows = new HashSet<DataRow>();
                foreach (var item in secondRs.Rows.Cast<DataRow>().Skip(1))
                {
                    foreach (DataRow row in rs.Rows)
                    {
                        var newRow = rs.Table.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        foreach (DataColumn column in secondRs.Columns)
                            newRow[initialColumnCount + column.Ordinal] = item[column.Ordinal];
                        newRows.Add(newRow);
                    }
                }
                foreach (var newRow in newRows)
                    rs.Table.Rows.Add(newRow);
            }
            rs.Table.AcceptChanges();
            return rs;
        }
    }
}
