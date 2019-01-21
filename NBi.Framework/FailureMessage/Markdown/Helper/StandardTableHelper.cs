using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Framework.Markdown.MarkdownLogExtension;
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
    class StandardTableHelper : BaseTableHelper<DataRow>
    {
        public StandardTableHelper(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> definitions)
            : base(rows, definitions) { }


        protected override TableExtended RenderNonEmptyTable()
        {
            var extendedDefinitions = ExtendDefinitions(Rows.ElementAt(0).Table, Metadatas);
            return new TableExtended() { Columns = RenderColumns(extendedDefinitions), Rows = RenderRows(Rows, extendedDefinitions) };
        }

        protected override IEnumerable<TableCellExtended> RenderRow(DataRow row, IEnumerable<ColumnType> columnTypes)
        {
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], columnTypes.ElementAt(i));
                yield return new TableCellExtended() { Text = displayValue };
            }
        }
    }
}
