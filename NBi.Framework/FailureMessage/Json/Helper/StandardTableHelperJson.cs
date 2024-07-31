using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Scalar.Presentation;
using NBi.Extensibility;
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
    class StandardTableHelperJson : BaseTableHelperJson<IResultRow>
    {
        public StandardTableHelperJson(IEnumerable<IResultRow> rows, IEnumerable<ColumnMetadata> definitions, ISampler<IResultRow> sampler)
            : base(rows, definitions, sampler) { }


        protected override void RenderNonEmptyTable(JsonWriter writer)
        {
            var extendedMetadata = ExtendMetadata(Rows.ElementAt(0).Parent, Metadatas);
            RenderNonEmptyTable(Rows, extendedMetadata, Sampler, writer);
        }

        protected override void RenderRow(IResultRow row, IEnumerable<ColumnType> columnTypes, JsonWriter writer)
        {
            writer.WriteStartArray();
            for (int i = 0; i < row.Parent.ColumnCount; i++)
            {
                RenderCell(row.IsNull(i) ? DBNull.Value : row.ItemArray[i]!, columnTypes.ElementAt(i), writer);
            }
            writer.WriteEndArray();
        }
    }
}


