using MarkdownLog;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup.Violation;
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
    class LookupTableHelper : BaseTableHelper<LookupMatchesViolationComposite>
    {
        public LookupTableHelper(IEnumerable<LookupMatchesViolationComposite> composites, IEnumerable<ColumnMetadata> metadatas)
            : base(composites, metadatas) { }

        protected override TableExtended RenderNonEmptyTable()
        {
            var extendedDefinitions = ExtendDefinitions(Rows.ElementAt(0).CandidateRow.Table, Metadatas);
            return new TableExtended() { Columns = RenderColumns(extendedDefinitions), Rows = RenderRows(Rows, extendedDefinitions) };
        }

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

        private TableRowExtended RenderFirstRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> definitions)
        {
            var cells = new List<TableCellExtended>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , definitions.ElementAt(i).Type);
                    cells.Add(new TableCellExtended() { Text = displayValue });
                }
                else
                {
                    var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], definitions.ElementAt(i).Type);
                    cells.Add(new TableCellExtended() { Text = displayValue });
                }
            }
            return new TableRowExtended() { Cells = cells }; 
        }

        private TableRowExtended RenderSupplementaryRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> definitions)
        {
            var cells = new List<TableCellExtended>();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , definitions.ElementAt(i).Type);
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
            var formatter = factory.Instantiate(columnType);
            return data.IsEqual ? formatter.Execute(value) : $"{formatter.Execute(value)} <> {formatter.Execute(data.Value)}";
        }

        protected override IEnumerable<TableCellExtended> RenderRow(LookupMatchesViolationComposite row, IEnumerable<ColumnType> columnTypes)
            => throw new NotImplementedException();
        
    }
}
