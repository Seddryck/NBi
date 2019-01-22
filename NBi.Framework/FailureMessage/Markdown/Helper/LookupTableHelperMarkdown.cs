using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup.Violation;
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
    class LookupTableHelperMarkdown : BaseTableHelperMarkdown<LookupMatchesViolationComposite>
    {
        public LookupTableHelperMarkdown(IEnumerable<LookupMatchesViolationComposite> composites, IEnumerable<ColumnMetadata> metadatas, ISampler<LookupMatchesViolationComposite> sampler)
            : base(composites, metadatas, sampler) { }


        protected override IEnumerable<ExtendedMetadata> BuildExtendedMetadatas(LookupMatchesViolationComposite row, IEnumerable<ColumnMetadata> metadatas)
                => ExtendMetadata(row.CandidateRow.Table, metadatas);

        protected override IEnumerable<TableRowExtended> RenderRows(IEnumerable<LookupMatchesViolationComposite> composites, IEnumerable<ExtendedMetadata> definitions)
        {
            foreach (var composite in composites)
            {
                var firstRecord = composite.Records.ElementAt(0);
                yield return RenderFirstRow(composite.CandidateRow, firstRecord, definitions);

                for (var i = 1; i < composite.Records.Count; i++)
                {
                    var record = composite.Records.ElementAt(i);
                    yield return RenderSupplementaryRow(composite.CandidateRow, record, definitions);
                }
            }
        }

        private TableRowExtended RenderFirstRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> metadatas)
        {
            var cells = new List<TableCellExtended>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , metadatas.ElementAt(i).Type);
                    cells.Add(new TableCellExtended() { Text = displayValue });
                }
                else
                {
                    var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], metadatas.ElementAt(i).Type);
                    cells.Add(new TableCellExtended() { Text = displayValue });
                }
            }
            return new TableRowExtended() { Cells = cells };
        }

        private TableRowExtended RenderSupplementaryRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> metadatas)
        {
            var cells = new List<TableCellExtended>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , metadatas.ElementAt(i).Type);
                    cells.Add(new TableCellExtended() { Text = displayValue });
                }
                else
                {
                    cells.Add(new TableCellExtended() { Text = RenderSupplementaryCell() });
                }
            }
            return new TableRowExtended() { Cells = cells };
        }

        private string RenderSupplementaryCell() => " >> ";

        protected virtual string RenderCell(object value, LookupMatchesViolationData data, ColumnType columnType)
        {
            var factory = new PresenterFactory();
            var presenter = factory.Instantiate(columnType);
            return data.IsEqual ? presenter.Execute(value) : $"{presenter.Execute(value)} <> {presenter.Execute(data.Value)}";
        }

        protected override IEnumerable<TableCellExtended> RenderRow(LookupMatchesViolationComposite row, IEnumerable<ColumnType> columnTypes)
            => throw new NotImplementedException();

    }
}
