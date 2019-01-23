using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Scalar.Presentation;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json.Helper
{
    class LookupTableHelperJson : BaseTableHelperJson<LookupMatchesViolationComposite>
    {
        public LookupTableHelperJson(IEnumerable<LookupMatchesViolationComposite> composites, IEnumerable<ColumnMetadata> metadatas, ISampler<LookupMatchesViolationComposite> sampler)
            : base(composites, metadatas, sampler) { }

        protected override void RenderNonEmptyTable(JsonWriter writer)
        {
            var extendedMetadatas = ExtendMetadata(Rows.ElementAt(0).CandidateRow.Table, Metadatas);
            RenderNonEmptyTable(Rows, extendedMetadatas, Sampler, writer);
        }

        protected override void RenderRows(IEnumerable<LookupMatchesViolationComposite> composites, IEnumerable<ExtendedMetadata> metadatas, JsonWriter writer)
        {
            writer.WritePropertyName("rows");
            writer.WriteStartArray();
            foreach (var composite in composites)
            {
                var firstRecord = composite.Records.ElementAt(0);
                RenderFirstRow(composite.CandidateRow, firstRecord, metadatas, writer);

                for (var i = 1; i < composite.Records.Count; i++)
                {
                    var record = composite.Records.ElementAt(i);
                    RenderSupplementaryRow(composite.CandidateRow, record, metadatas, writer);
                }
            }
            writer.WriteEndArray(); //rows
        }

        private void RenderFirstRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> metadatas, JsonWriter writer)
        {
            writer.WriteStartArray();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , metadatas.ElementAt(i).Type);
                    writer.WriteValue(displayValue);
                }
                else
                {
                    var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], metadatas.ElementAt(i).Type);
                    writer.WriteValue(displayValue);
                }
            }
            writer.WriteEndArray();
        }

        private void RenderSupplementaryRow(DataRow row, LookupMatchesViolationRecord record, IEnumerable<ExtendedMetadata> metadatas, JsonWriter writer)
        {
            writer.WriteStartArray();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (record.ContainsKey(row.Table.Columns[i]))
                {
                    var displayValue = RenderCell(
                        row.IsNull(i) ? DBNull.Value : row.ItemArray[i]
                        , record[row.Table.Columns[i]]
                        , metadatas.ElementAt(i).Type);
                    writer.WriteValue(displayValue);
                }
                else
                {
                    var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], metadatas.ElementAt(i).Type);
                    writer.WriteValue(displayValue);
                }
            }
            writer.WriteEndArray();
        }

        private string RenderSupplementaryCell() => " >> ";

        protected virtual string RenderCell(object value, LookupMatchesViolationData data, ColumnType columnType)
        {
            var factory = new PresenterFactory();
            var formatter = factory.Instantiate(columnType);
            return data.IsEqual ? formatter.Execute(value) : $"{formatter.Execute(value)} <> {formatter.Execute(data.Value)}";
        }

        protected override void RenderRow(LookupMatchesViolationComposite row, IEnumerable<ColumnType> columnTypes, JsonWriter writer)
            => throw new NotImplementedException();

    }
}

