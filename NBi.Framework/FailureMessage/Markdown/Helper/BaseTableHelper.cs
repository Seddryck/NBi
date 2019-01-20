using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Framework.Markdown.MarkdownLogExtension;
using NBi.Unit.Framework.FailureMessage.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper
{
    abstract class BaseTableHelper<T> : ITableHelper
    {
        public IReadOnlyCollection<T> Rows { get; }
        public IEnumerable<IColumnDefinition> Definitions { get; }

        public BaseTableHelper(IEnumerable<T> rows, IEnumerable<IColumnDefinition> definitions)
            => (Rows, Definitions) = (new ReadOnlyCollection<T>(rows.ToList()), definitions);

        public MarkdownContainer Render()
        {
            var container = new MarkdownContainer();
            container.Append(Rows.Count() == 0 ? (IMarkdownElement) RenderEmptyTable() : RenderNonEmptyTable());
            return container;
        }

        protected virtual Paragraph RenderEmptyTable() => "This result-set is empty.".ToMarkdownParagraph();

        protected abstract TableExtended RenderNonEmptyTable();

        protected virtual IEnumerable<TableRowExtended> RenderRows(IEnumerable<T> rows, IEnumerable<ExtendedColumnDefinition> columnDefinitions)
        {
            foreach (var row in rows)
            {
                var cells = RenderRow(row, columnDefinitions.Select(x => x.Type));
                yield return new TableRowExtended() { Cells = cells };
            }
        }

        protected abstract IEnumerable<TableCellExtended> RenderRow(T row, IEnumerable<ColumnType> columnTypes);

        protected virtual string RenderCell(object value, ColumnType columnType)
        {
            var factory = new CellFormatterFactory();
            var formatter = factory.Instantiate(columnType);
            return formatter.Format(value);
        }

        protected IEnumerable<TableColumnExtended> RenderColumns(IEnumerable<ExtendedColumnDefinition> columnDefinitions)
        {
            var formatter = new ColumnPropertiesFormatter();
            foreach (var definition in columnDefinitions)
            {
                var tableColumn = new TableColumnExtended()
                {
                    HeaderCell = new TableCellExtended()
                    { Text = (definition.Identifier)==null ? $"#{definition.Ordinal} ({definition.Name})" :  $"{definition.Identifier.Label}" },
                    SubHeaderCell = new TableCellExtended() { Text = formatter.GetText(definition) }
                };
                yield return tableColumn;
            }
        }

        protected internal virtual IEnumerable<ExtendedColumnDefinition> ExtendDefinitions(DataTable table, IEnumerable<IColumnDefinition> existingDefinitions)
        {
            var definedColumns = new Dictionary<DataColumn, IColumnDefinition>();
            foreach (var definition in existingDefinitions)
                definedColumns.Add(table.GetColumn(definition.Identifier), definition);

            var identifierFactory = new ColumnIdentifierFactory();
            foreach (DataColumn dataColumn in table.Columns)
            {
                var definition = definedColumns.ContainsKey(dataColumn) 
                    ? new ExtendedColumnDefinition()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.ColumnName,
                        Role = definedColumns[dataColumn].Role,
                        Type = definedColumns[dataColumn].Type
                    }
                    : new ExtendedColumnDefinition()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.ColumnName,
                        Role = ColumnRole.Ignore,
                        Type = ColumnType.Text
                    };
                yield return definition;
            }
        }

        protected internal class ExtendedColumnDefinition : Column
        {
            public string Name { get; set; }
            public int Ordinal { get; set; }
        }


        //protected override List<TableRowExtended> BuildRows(IEnumerable<DataRow> dataRows, List<ColumnType> columnTypes)
        //{
        //    var rows = new List<TableRowExtended>();
        //    foreach (DataRow dataRow in dataRows)
        //    {
        //        var cells = new List<TableCellExtended>();
        //        for (int i = 0; i < dataRow.Table.Columns.Count; i++)
        //        {
        //            var text = GetText(columnTypes, dataRow, i);
        //            var compared = GetCompareText(columnTypes, dataRow, i);
        //            var fullText = string.Format("{0}{1}{2}", text, string.IsNullOrEmpty(compared) ? "" : " <> ", compared);
        //            cells.Add(new TableCellExtended() { Text = fullText });
        //        }
        //        rows.Add(new TableRowExtended() { Cells = cells });
        //    }
        //    return rows;
        //}

        //protected string GetCompareText(List<ColumnType> columnTypes, DataRow dataRow, int i)
        //{
        //    if (string.IsNullOrEmpty(dataRow.GetColumnError(i)))
        //        return string.Empty;

        //    var factory = new CellFormatterFactory();
        //    var formatter = factory.GetObject(columnTypes[i]);

        //    return formatter.Format(dataRow.GetColumnError(i));
        //}
    }
}
