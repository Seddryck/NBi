using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Framework.Sampling;
using NBi.Unit.Framework.FailureMessage.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json
{
    class CompareTableHelperJson : TableHelperJson
    {
        protected override void BuildRows(IEnumerable<DataRow> rows, IEnumerable<CellFormatter> formatters, JsonWriter writer)
        {
            writer.WritePropertyName("rows");
            writer.WriteStartArray();
            foreach (DataRow row in rows)
            {
                writer.WriteStartArray();
                for (int i = 0; i < row.ItemArray.Count(); i++)
                {
                    var formatter = formatters.ElementAt(i);
                    writer.WriteStartObject();
                    writer.WritePropertyName("value");
                    writer.WriteValue(formatter.Format(row[i]));
                    if (!string.IsNullOrEmpty(row.GetColumnError(i)))
                    {
                        writer.WritePropertyName("expectation");
                        writer.WriteValue(formatter.Format(row.GetColumnError(i)));
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray(); //rows
        }
    }
}

