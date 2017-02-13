using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.Framework.MarkdownLogExtension;
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
    class TableHelper
    {
        
        private readonly ComparisonStyle style;
        public TableHelper(ComparisonStyle style)
        {
            this.style = style;
        }

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

        protected TableExtended BuildNonEmptyTable(IEnumerable<DataRow> dataRows)
        {
            List<ColumnType> columnTypes;
            var headers = BuildColumns(dataRows, out columnTypes);
            var rows = BuildRows(dataRows, columnTypes);

            return new TableExtended() { Columns = headers, Rows = rows };
        }

        protected virtual List<TableRowExtended> BuildRows(IEnumerable<DataRow> dataRows, List<ColumnType> columnTypes)
        {
            var rows = new List<TableRowExtended>();
            foreach (DataRow dataRow in dataRows)
            {
                var cells = new List<TableCellExtended>();
                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    var text = GetText(columnTypes, dataRow, i);
                    cells.Add(new TableCellExtended() { Text = text });
                }

                rows.Add(new TableRowExtended() { Cells = cells });
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

        private List<TableColumnExtended> BuildColumns(IEnumerable<DataRow> dataRows, out List<ColumnType> columnTypes)
        {
            var headers = new List<TableColumnExtended>();
            columnTypes = new List<ColumnType>();
            foreach (DataColumn dataColumn in dataRows.ElementAt(0).Table.Columns)
            {
                var formatter = new TableHeaderHelper();
                var tableColumn = new TableColumnExtended();
                var headerCell = new TableCellExtended() {  };
                switch (style)
                {
                    case ComparisonStyle.ByIndex:
                        headerCell.Text = string.Format("#{0} ({1})", headers.Count, dataColumn.ColumnName);
                        break;
                    case ComparisonStyle.ByName:
                        headerCell.Text = string.Format("{0}", dataColumn.ColumnName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                tableColumn.HeaderCell = headerCell;
                
                if (dataColumn.ExtendedProperties.Count > 0)
                {
                    var role = (ColumnRole)(dataColumn.ExtendedProperties["NBi::Role"] ?? ColumnRole.Key);
                    var type = (ColumnType)(dataColumn.ExtendedProperties["NBi::Type"] ?? ColumnType.Text);
                    var tolerance = (Tolerance)(dataColumn.ExtendedProperties["NBi::Tolerance"]);
                    var rounding = (Rounding)(dataColumn.ExtendedProperties["NBi::Rounding"]);
                    columnTypes.Add(type);

                    var subHeader = formatter.GetText(role, type, tolerance, rounding);
                    var subHeaderCell = new TableCellExtended() { Text = subHeader };
                    tableColumn.SubHeaderCell = subHeaderCell;
                }
                else
                    columnTypes.Add(ColumnType.Text);

                headers.Add(tableColumn);
            }

            return headers;
        }
    }
}
