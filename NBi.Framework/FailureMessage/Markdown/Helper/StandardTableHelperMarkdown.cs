using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
using NBi.Framework.Markdown.MarkdownLogExtension;
using NBi.Framework.Sampling;
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
    class StandardTableHelperMarkdown : BaseTableHelperMarkdown<DataRow>
    {
        public StandardTableHelperMarkdown(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> definitions, ISampler<DataRow> sampler)
            : base(rows, definitions, sampler) { }

        protected override IEnumerable<ExtendedMetadata> BuildExtendedMetadatas(DataRow row, IEnumerable<ColumnMetadata> metadatas)
            => ExtendMetadata(row.Table, Metadatas);

        protected override IEnumerable<TableCellExtended> RenderRow(DataRow row, IEnumerable<ColumnType> columnTypes)
        {
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], columnTypes.ElementAt(i));
                yield return new TableCellExtended() { Text = displayValue };
            }
        }

        protected virtual string RenderCell(object value, ColumnType columnType)
        {
            var factory = new PresenterFactory();
            var presenter = factory.Instantiate(columnType);
            return $"{presenter.Execute(value)}";
        }
    }
}
