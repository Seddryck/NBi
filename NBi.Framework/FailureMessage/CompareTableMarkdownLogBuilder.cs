using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Unit.Framework.FailureMessage.Formatter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public class CompareTableMarkdownLogBuilder : TableMarkdownLogBuilder
    {
        protected override List<TableRow> BuildRows(IEnumerable<DataRow> dataRows, List<ColumnType> columnTypes)
        {
            var rows = new List<TableRow>();
            foreach (DataRow dataRow in dataRows)
            {
                var cells = new List<TableCell>();
                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    var text = GetText(columnTypes, dataRow, i);
                    var compared = GetCompareText(columnTypes, dataRow, i);
                    var fullText = string.Format("{0}{1}{2}", text, string.IsNullOrEmpty(compared) ? "" : " <> ", compared);
                    cells.Add(new TableCell() { Text = fullText });
                }
                rows.Add(new TableRow() { Cells = cells });
            }
            return rows;
        }

        protected string GetCompareText(List<ColumnType> columnTypes, DataRow dataRow, int i)
        {
            if (string.IsNullOrEmpty(dataRow.GetColumnError(i)))
                return string.Empty;
            
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(columnTypes[i]);

            return formatter.Format(dataRow.GetColumnError(i));
        }
    }
}
