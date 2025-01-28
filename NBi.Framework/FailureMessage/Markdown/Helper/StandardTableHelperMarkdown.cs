using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
using NBi.Extensibility;
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

namespace NBi.Framework.FailureMessage.Markdown.Helper;

class StandardTableHelperMarkdown : BaseTableHelperMarkdown<IResultRow>
{
    public StandardTableHelperMarkdown(IEnumerable<IResultRow> rows, IEnumerable<ColumnMetadata> definitions, ISampler<IResultRow> sampler)
        : base(rows, definitions, sampler) { }

    protected override IEnumerable<ExtendedMetadata> BuildExtendedMetadatas(IResultRow row, IEnumerable<ColumnMetadata> metadatas)
        => ExtendMetadata(row.Parent, Metadatas);

    protected override IEnumerable<TableCellExtended> RenderRow(IResultRow row, IEnumerable<ColumnType> columnTypes)
    {
        for (int i = 0; i < row.Parent.ColumnCount; i++)
        {
            var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i]!, columnTypes.ElementAt(i));
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
