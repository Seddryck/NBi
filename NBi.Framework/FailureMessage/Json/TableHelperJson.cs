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
    class TableHelperJson
    {
        public void Execute(IEnumerable<DataRow> rows, ISampler<DataRow> sampler, JsonWriter writer)
        {
            rows = rows ?? new List<DataRow>();
            sampler.Build(rows);
            var sampled = sampler.GetResult();

            writer.WriteStartObject();
            writer.WritePropertyName("total-rows");
            writer.WriteValue(rows.Count());
            if (sampler.GetIsSampled())
            {
                writer.WritePropertyName("sampled-rows");
                writer.WriteValue(rows.Count() - sampler.GetExcludedRowCount());
            }

            if (sampled.Count() > 0)
            {
                writer.WritePropertyName("table");
                writer.WriteStartObject();

                writer.WritePropertyName("columns");
                writer.WriteStartArray();
                var formatters = new List<CellFormatter>();

                foreach (DataColumn column in sampled.ElementAt(0).Table.Columns)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("position");
                    writer.WriteValue(column.Ordinal);
                    writer.WritePropertyName("name");
                    writer.WriteValue(column.ColumnName);

                    var cpFormatter = new ColumnPropertiesFormatter();
                    writer.WritePropertyName("role");
                    writer.WriteValue(cpFormatter.GetRoleText((ColumnRole)(column.ExtendedProperties["NBi::Role"] ?? ColumnRole.Key)));
                    writer.WritePropertyName("type");
                    writer.WriteValue(cpFormatter.GetTypeText((ColumnType)(column.ExtendedProperties["NBi::Type"] ?? ColumnType.Text)));
                    formatters.Add(new CellFormatterFactory().GetObject((ColumnType)(column.ExtendedProperties["NBi::Type"] ?? ColumnType.Text)));
                    var tolerance = (Tolerance)(column.ExtendedProperties["NBi::Tolerance"]);
                    if (!Tolerance.IsNullOrNone(tolerance))
                    {
                        writer.WritePropertyName("tolerance");
                        writer.WriteValue(cpFormatter.GetToleranceText(tolerance).Trim());
                    }
                    var rounding = (Rounding)(column.ExtendedProperties["NBi::Rounding"]);
                    if (rounding != null)
                    {
                        writer.WritePropertyName("rounding");
                        writer.WriteValue(cpFormatter.GetRoundingText(rounding));
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray(); //columns

                BuildRows(sampled, formatters, writer);
                
                writer.WriteEndObject(); //table
            }
            writer.WriteEndObject();
        }

        protected virtual void BuildRows(IEnumerable<DataRow> rows, IEnumerable<CellFormatter> formatters, JsonWriter writer)
        {
            writer.WritePropertyName("rows");
            writer.WriteStartArray();
            foreach (DataRow row in rows)
            {
                
                writer.WriteStartArray();
                for (int i = 0; i < row.ItemArray.Count(); i++)
                {
                    var value = formatters.ElementAt(i).Format(row[i]);
                    writer.WriteValue(value);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray(); //rows
        }
    }
}

