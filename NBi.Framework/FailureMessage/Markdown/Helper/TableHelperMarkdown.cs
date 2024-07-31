using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Scalar.Presentation;
using NBi.Extensibility;
using NBi.Framework.Markdown.MarkdownLogExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper
{
    class TableHelperMarkdown
    {
        
        private readonly EngineStyle style;
        public TableHelperMarkdown(EngineStyle style)
        {
            this.style = style;
        }

        public MarkdownContainer Build(IEnumerable<IResultRow> dataRows)
        {
            var container = new MarkdownContainer();

            if (!dataRows.Any())
                container.Append(BuildEmptyTable());
            else
                container.Append(BuildNonEmptyTable(dataRows));
                
            return container;
        }

        protected virtual Paragraph BuildEmptyTable()
            => "This result set is empty.".ToMarkdownParagraph();

        protected TableExtended BuildNonEmptyTable(IEnumerable<IResultRow> dataRows)
        {
            var headers = BuildColumns(dataRows, out var columnTypes);
            var rows = BuildRows(dataRows, columnTypes);

            return new TableExtended() { Columns = headers, Rows = rows };
        }

        protected virtual List<TableRowExtended> BuildRows(IEnumerable<IResultRow> dataRows, List<ColumnType> columnTypes)
        {
            var rows = new List<TableRowExtended>();
            foreach (IResultRow dataRow in dataRows)
            {
                var cells = new List<TableCellExtended>();
                for (int i = 0; i < dataRow.Parent.ColumnCount; i++)
                {
                    var text = GetText(columnTypes, dataRow, i);
                    cells.Add(new TableCellExtended() { Text = text });
                }

                rows.Add(new TableRowExtended() { Cells = cells });
            }
            return rows;
        }

        protected virtual string GetText(List<ColumnType> columnTypes, IResultRow dataRow, int i)
        {
            var factory = new PresenterFactory();
            var formatter = factory.Instantiate(columnTypes[i]);

            var text = dataRow.IsNull(i)
                        ? formatter.Execute(DBNull.Value)
                        : formatter.Execute(dataRow.ItemArray[i]);
            return text;
        }

        private List<TableColumnExtended> BuildColumns(IEnumerable<IResultRow> dataRows, out List<ColumnType> columnTypes)
        {
            var headers = new List<TableColumnExtended>();
            columnTypes = [];
            foreach (var dataColumn in dataRows.ElementAt(0).Parent.Columns)
            {
                var formatter = new ColumnPropertiesFormatter();
                var tableColumn = new TableColumnExtended();
                var headerCell = new TableCellExtended() {  };
                headerCell.Text = style switch
                {
                    EngineStyle.ByIndex => $"#{headers.Count} ({dataColumn.Name})",
                    EngineStyle.ByName => $"{dataColumn.Name}",
                    _ => throw new ArgumentOutOfRangeException(),
                };
                tableColumn.HeaderCell = headerCell;
                
                if (dataColumn.HasProperties())
                {
                    var role = (ColumnRole)(dataColumn.GetProperty("Role") ?? ColumnRole.Key);
                    var type = (ColumnType)(dataColumn.GetProperty("Type") ?? ColumnType.Text);
                    var tolerance = (Tolerance?)(dataColumn.GetProperty("Tolerance"));
                    var rounding = (Rounding?)(dataColumn.GetProperty("Rounding"));
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
