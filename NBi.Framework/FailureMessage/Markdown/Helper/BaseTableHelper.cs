using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
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
    abstract class BaseTableHelper<T> : ITableHelper
    {
        public IReadOnlyCollection<T> Rows { get; }
        public IEnumerable<ColumnMetadata> Metadatas { get; }

        public BaseTableHelper(IEnumerable<T> rows, IEnumerable<ColumnMetadata> metadata)
            => (Rows, Metadatas) = (new ReadOnlyCollection<T>(rows.ToList()), metadata);

        public MarkdownContainer Render()
        {
            var container = new MarkdownContainer();
            container.Append(Rows.Count() == 0 ? (IMarkdownElement) RenderEmptyTable() : RenderNonEmptyTable());
            return container;
        }

        protected virtual Paragraph RenderEmptyTable() => "This result-set is empty.".ToMarkdownParagraph();

        protected abstract TableExtended RenderNonEmptyTable();

        protected virtual IEnumerable<TableRowExtended> RenderRows(IEnumerable<T> rows, IEnumerable<ExtendedMetadata> columnDefinitions)
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
            var factory = new PresenterFactory();
            var formatter = factory.Instantiate(columnType);
            return formatter.Execute(value);
        }

        protected IEnumerable<TableColumnExtended> RenderColumns(IEnumerable<ExtendedMetadata> metadatas)
        {
            var formatter = new ColumnPropertiesFormatter();
            foreach (var metadata in metadatas)
            {
                var tableColumn = new TableColumnExtended()
                {
                    HeaderCell = new TableCellExtended()
                    { Text = (metadata.Identifier)==null ? $"#{metadata.Ordinal} ({metadata.Name})" :  $"{metadata.Identifier.Label}" },
                    SubHeaderCell = new TableCellExtended() { Text = formatter.GetText(metadata) }
                };
                yield return tableColumn;
            }
        }

        protected internal virtual IEnumerable<ExtendedMetadata> ExtendDefinitions(DataTable table, IEnumerable<ColumnMetadata> existingDefinitions)
        {
            var metadataDico = new Dictionary<DataColumn, ColumnMetadata>();
            foreach (var definition in existingDefinitions)
                metadataDico.Add(table.GetColumn(definition.Identifier), definition);

            var identifierFactory = new ColumnIdentifierFactory();
            foreach (DataColumn dataColumn in table.Columns)
            {
                var metadata = metadataDico.ContainsKey(dataColumn) 
                    ? new ExtendedMetadata()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.ColumnName,
                        Role = metadataDico[dataColumn].Role,
                        Type = metadataDico[dataColumn].Type
                    }
                    : new ExtendedMetadata()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.ColumnName,
                        Role = ColumnRole.Ignore,
                        Type = ColumnType.Text
                    };
                yield return metadata;
            }
        }

        protected internal class ExtendedMetadata : ColumnMetadata
        {
            public string Name { get; set; }
            public int Ordinal { get; set; }
        }
    }
}
