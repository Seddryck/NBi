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

namespace NBi.Framework.FailureMessage.Helper
{
    public class TableHelper
    {
        public MarkdownContainer Build(IEnumerable<DataRow> dataRows)
        {
            var container = new MarkdownContainer();

            if (dataRows.Count() == 0)
                container.Append(BuildEmptyTable());
            else
                container.Append(BuildNonEmptyTable(dataRows));
                
            return container;
        }

        protected Paragraph BuildEmptyTable()
        {
            return "This result set is empty.".ToMarkdownParagraph();
        }

        protected Table BuildNonEmptyTable(IEnumerable<DataRow> dataRows)
        {
            List<ColumnType> columnTypes;
            var headers = BuildColumns(dataRows, out columnTypes);
            var rows = BuildRows(dataRows, columnTypes);

            return new Table() { Columns = headers, Rows = rows };
        }

        protected virtual List<TableRow> BuildRows(IEnumerable<DataRow> dataRows, List<ColumnType> columnTypes)
        {
            var rows = new List<TableRow>();
            foreach (DataRow dataRow in dataRows)
            {
                var cells = new List<TableCell>();
                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    var text = GetText(columnTypes, dataRow, i);
                    cells.Add(new TableCell() { Text = text });
                }

                rows.Add(new TableRow() { Cells = cells });
            }
            return rows;
        }

        protected string GetText(List<ColumnType> columnTypes, DataRow dataRow, int i)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.GetObject(columnTypes[i]);

            var text = string.Empty;
            if (dataRow.IsNull(i))
                text = formatter.Format(DBNull.Value);
            else
                text = formatter.Format(dataRow.ItemArray[i]);
            return text;
        }

        private List<TableColumn> BuildColumns(IEnumerable<DataRow> dataRows, out List<ColumnType> columnTypes)
        {
            var headers = new List<TableColumn>();
            columnTypes = new List<ColumnType>();
            foreach (DataColumn dataColumn in dataRows.ElementAt(0).Table.Columns)
            {
                var formatter = new TableHeaderHelper();

                var header = string.Empty;
                if (dataColumn.ExtendedProperties.Count == 0)
                {
                    columnTypes.Add(ColumnType.Text);
                    header = dataColumn.ColumnName;
                }
                else
                {
                    var role = (ColumnRole)(dataColumn.ExtendedProperties["NBi::Role"] ?? ColumnRole.Key);
                    var type = (ColumnType)(dataColumn.ExtendedProperties["NBi::Type"] ?? ColumnType.Text);
                    var tolerance = (Tolerance)(dataColumn.ExtendedProperties["NBi::Tolerance"]);
                    var rounding = (Rounding)(dataColumn.ExtendedProperties["NBi::Rounding"]);
                    columnTypes.Add(type);

                    header = formatter.GetText(role, type, tolerance, rounding);
                }
                headers.Add(new TableColumn() { HeaderCell = new TableCell() { Text = header } });
            }

            return headers;
        }
    }
}
