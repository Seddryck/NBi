using NBi.Core.ResultSet;
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
    class StandardTableHelperJson : BaseTableHelperJson<DataRow>
    {
        public StandardTableHelperJson(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> definitions, ISampler<DataRow> sampler)
            : base(rows, definitions, sampler) { }


        protected override void RenderNonEmptyTable(JsonWriter writer)
        {
            var extendedMetadata = ExtendMetadata(Rows.ElementAt(0).Table, Metadatas);
            RenderNonEmptyTable(Rows, extendedMetadata, Sampler, writer);
        }

        protected override void RenderRow(DataRow row, IEnumerable<ColumnType> columnTypes, JsonWriter writer)
        {
            writer.WriteStartArray();
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                var displayValue = RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i], columnTypes.ElementAt(i));
                writer.WriteValue(displayValue);
            }
            writer.WriteEndArray();
        }
    }
}


