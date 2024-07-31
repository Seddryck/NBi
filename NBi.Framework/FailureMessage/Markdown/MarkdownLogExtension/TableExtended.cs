using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Markdown.MarkdownLogExtension
{
    class TableExtended : MarkdownElement
    {

        private static readonly EmptyTableCellExtended EmptyCell = new EmptyTableCellExtended();
        
        private IEnumerable<TableRowExtended> _rows = new List<TableRowExtended>();
        private IEnumerable<TableColumnExtended> _columns = new List<TableColumnExtended>();

        public IEnumerable<TableRowExtended> Rows
        {
            get { return _rows; }
            set { _rows = value ?? Enumerable.Empty<TableRowExtended>(); }
        }

        public IEnumerable<TableColumnExtended> Columns
        {
            get { return _columns; }
            set { _columns = value ?? new List<TableColumnExtended>(); }
        }

        public override string ToMarkdown()
        {
            var markdownBuilder = new MarkdownBuilder(this);
            return markdownBuilder.Build();
        }

        private class MarkdownBuilder
        {
            private class Row
            {
                public IList<ITableCellExtended> Cells { get; set; } = [];
            }

            private readonly List<Row> _rows;
            private readonly List<TableColumnExtended> _columns;
            private readonly StringBuilder _builder = new StringBuilder();
            private readonly IList<TableCellRenderSpecificationExtended> _columnRenderSpecs;

            internal MarkdownBuilder(TableExtended table)
            {
                _columns = table.Columns.Cast<TableColumnExtended>().ToList();
                _rows = table.Rows.Select(row => new Row { Cells = row.Cells.ToList() }).ToList();

                var columnCount = Math.Max(_columns.Count, _rows.Any() ? _rows.Max(r => r.Cells.Count) : 0);
                _columnRenderSpecs = Enumerable.Range(0, columnCount).Select(BuildColumnSpecification).ToList();
            }

            private TableCellRenderSpecificationExtended BuildColumnSpecification(int column)
            {
                return new TableCellRenderSpecificationExtended(GetColumnAt(column).Alignment, GetMaximumCellWidth(column));
            }

            internal string Build()
            {
                BuildHeaderRow();
                BuildSubHeaderRow();
                BuildDividerRow();

                foreach (var row in _rows)
                {
                    BuildBodyRow(row);
                }

                return _builder.ToString();
            }



            private void BuildHeaderRow()
            {
                var headerCells = (from column in Enumerable.Range(0, _columnRenderSpecs.Count)
                                   let cell = GetColumnAt(column).HeaderCell
                                   let text = BuildCellMarkdownCode(column, cell)
                                   select text).ToList();

                _builder.Append("    ");
                _builder.AppendLine(" " + string.Join(" | ", headerCells));
            }

            private void BuildSubHeaderRow()
            {
                var headerCells = (from column in Enumerable.Range(0, _columnRenderSpecs.Count)
                                   let cell = GetColumnAt(column).SubHeaderCell
                                   let text = BuildCellMarkdownCode(column, cell)
                                   select text).ToList();

                if (headerCells.All(h => h.Trim() == string.Empty))
                    return;

                _builder.Append("    ");
                _builder.AppendLine(" " + string.Join(" | ", headerCells));
            }

            private void BuildDividerRow()
            {
                _builder.Append("    ");
                _builder.AppendLine(string.Join("|", _columnRenderSpecs.Select(BuildDividerCell)));
            }

            private static string BuildDividerCell(TableCellRenderSpecificationExtended spec)
            {
                var dashes = new string('-', spec.MaximumWidth);

                switch (spec.Alignment)
                {
                    case TableColumnAlignment.Left:
                        return ":" + dashes + " ";
                    case TableColumnAlignment.Center:
                        return ":" + dashes + ":";
                    case TableColumnAlignment.Right:
                        return " " + dashes + ":";
                    default:
                        return " " + dashes + " ";
                }
            }

            private void BuildBodyRow(Row row)
            {
                var rowCells = (from column in Enumerable.Range(0, _columnRenderSpecs.Count)
                                let cell = GetCellAt(row.Cells, column)
                                select BuildCellMarkdownCode(column, cell)).ToList();

                _builder.Append("    ");
                _builder.AppendLine(" " + string.Join(" | ", rowCells));
            }

            private string BuildCellMarkdownCode(int column, ITableCellExtended cell)
            {
                var columnSpec = _columnRenderSpecs[column];
                var maximumWidth = columnSpec.MaximumWidth;

                var cellText = cell.BuildCodeFormattedString(new TableCellRenderSpecificationExtended(columnSpec.Alignment, maximumWidth));
                var truncatedCellText = cellText.Length > maximumWidth ? cellText.Substring(0, maximumWidth) : cellText.PadRight(maximumWidth);

                return truncatedCellText;
            }

            private int GetMaximumCellWidth(int column)
            {
                var headerCells = new[] { GetColumnAt(column).HeaderCell };
                var subHeaderCells = new[] { GetColumnAt(column).SubHeaderCell };
                var bodyCells = _rows.Select(row => GetCellAt(row.Cells, column));
                var columnCells = headerCells.Concat(subHeaderCells).Concat(bodyCells);
                return columnCells.Max(i => i.RequiredWidth);
            }
            private TableColumnExtended GetColumnAt(int index)
            {
                return index < _columns.Count
                    ? (_columns[index] as TableColumnExtended)
                    : CreateDefaultHeaderCell(index);
            }
            
            private static TableColumnExtended CreateDefaultHeaderCell(int columnIndex)
            {
                // GitHub Flavoured Markdown requires a header cell. If header text isn't provided
                // use an Excel-like naming scheme (e.g. A, B, C, .., AA, AB, etc)

                return new TableColumnExtended { HeaderCell = new TableCellExtended { Text = columnIndex.ToColumnTitle() } };
            }

            private ITableCellExtended GetCellAt(IList<ITableCellExtended> cells, int index)
            {
                return index < cells.Count ? cells[index] : EmptyCell;
            }
        }
    }
}
