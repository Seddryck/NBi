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

namespace NBi.Framework.FailureMessage.Markdown.Helper;

abstract class BaseTableHelperMarkdown<T> : Common.Helper.BaseTableHelper<T, MarkdownContainer>, Common.Helper.ITableHelper<MarkdownContainer>
{
    public BaseTableHelperMarkdown(IEnumerable<T> rows, IEnumerable<ColumnMetadata> metadatas, ISampler<T> sampler)
        : base(rows, metadatas, sampler) { }

    public override void Render(MarkdownContainer container)
    {
        if (Rows.Count == 0)
            RenderEmptyTable(container);
        else
            RenderNonEmptyTable(Rows, Metadatas, Sampler, container);
    }

    protected virtual void RenderEmptyTable(MarkdownContainer container)
        => container.Append("This result-set is empty.".ToMarkdownParagraph());

    protected virtual void RenderNonEmptyTable(IEnumerable<T> rows, IEnumerable<ColumnMetadata> metadatas, ISampler<T> sampler, MarkdownContainer container)
    {
        var extendedDefinitions = BuildExtendedMetadatas(rows.ElementAt(0), metadatas);

        container.Append($"Result-set with {rows.Count()} row{(rows.Count() > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());
        container.Append(new TableExtended() { Columns = RenderColumns(extendedDefinitions), Rows = RenderRows(Sampler.GetResult(), extendedDefinitions) });

        if (Sampler.GetIsSampled())
        {
            var rowsSkipped = $"{Sampler.GetExcludedRowCount()} (of {Rows.Count}) rows have been skipped for display purpose.";
            container.Append(rowsSkipped.ToMarkdownParagraph());
        }
    }

    protected abstract IEnumerable<ExtendedMetadata> BuildExtendedMetadatas(T row, IEnumerable<ColumnMetadata> metadatas);

    protected virtual IEnumerable<TableRowExtended> RenderRows(IEnumerable<T> rows, IEnumerable<ExtendedMetadata> extendedMetadata)
    {
        foreach (var row in rows)
        {
            var cells = RenderRow(row, extendedMetadata.Select(x => x.Type));
            yield return new TableRowExtended() { Cells = cells };
        }
    }

    protected abstract IEnumerable<TableCellExtended> RenderRow(T row, IEnumerable<ColumnType> columnTypes);

    protected virtual IEnumerable<TableColumnExtended> RenderColumns(IEnumerable<ExtendedMetadata> metadatas)
    {
        var formatter = new ColumnPropertiesFormatter();
        foreach (var metadata in metadatas)
        {
            var tableColumn = new TableColumnExtended()
            {
                HeaderCell = new TableCellExtended()
                { Text = (metadata.Identifier) == null ? $"#{metadata.Ordinal} ({metadata.Name})" : $"{metadata.Identifier.Label}" },
                SubHeaderCell = new TableCellExtended() { Text = formatter.GetText(metadata) }
            };
            yield return tableColumn;
        }
    }
}
