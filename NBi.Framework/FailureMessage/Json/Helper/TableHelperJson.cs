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

namespace NBi.Framework.FailureMessage.Json
{
    class TableHelperJson
    {
        public void Execute(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, JsonWriter writer)
        {
            rows ??= [];
            Execute(rows, sampler, BuildMetadataFromTable(rows.Any() ? rows.ElementAt(0)!.Parent : null), writer);
        }
            

        protected virtual IEnumerable<ColumnMetadata> BuildMetadataFromTable(IResultSet? table)
        {
            if (table == null)
                yield break;

            foreach (var column in table.Columns)
            {
                yield return new ColumnMetadata()
                {
                    Role = (ColumnRole)(column.GetProperty("Role") ?? ColumnRole.Key),
                    Type = (ColumnType)(column.GetProperty("Type") ?? ColumnType.Text),
                    Tolerance = (Tolerance?)(column.GetProperty("Tolerance")),
                    Rounding = (Rounding?)(column.GetProperty("Rounding"))
                };
            }
        }

        public void Execute(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, IEnumerable<ColumnMetadata> metadata, JsonWriter writer)
        {
            rows ??= [];
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

            if (sampled.Any())
            {
                writer.WritePropertyName("table");
                writer.WriteStartObject();

                writer.WritePropertyName("columns");
                writer.WriteStartArray();
                var formatters = new List<IPresenter>();

                var table = sampled.ElementAt(0).Parent;
                for (var i = 0; i < table.ColumnCount; i++)
                {
                    var meta = metadata.ElementAt(i);

                    writer.WriteStartObject();
                    writer.WritePropertyName("position");
                    writer.WriteValue(table.GetColumn(i)?.Ordinal);
                    writer.WritePropertyName("name");
                    writer.WriteValue(table.GetColumn(i)?.Name);

                    var cpFormatter = new ColumnPropertiesFormatter();
                    writer.WritePropertyName("role");
                    writer.WriteValue(cpFormatter.GetRoleText(meta.Role));
                    writer.WritePropertyName("type");
                    writer.WriteValue(cpFormatter.GetTypeText(meta.Type));
                    if (!Tolerance.IsNullOrNone(meta.Tolerance))
                    {
                        writer.WritePropertyName("tolerance");
                        writer.WriteValue(cpFormatter.GetToleranceText(meta.Tolerance).Trim());
                    }
                    if (meta.Rounding != null)
                    {
                        writer.WritePropertyName("rounding");
                        writer.WriteValue(cpFormatter.GetRoundingText(meta.Rounding));
                    }

                    formatters.Add(new PresenterFactory().Instantiate(metadata.ElementAt(i).Type));
                    writer.WriteEndObject();
                }
                writer.WriteEndArray(); //columns

                BuildRows(sampled, formatters, writer);

                writer.WriteEndObject(); //table
            }
            writer.WriteEndObject();
        }

        protected virtual void BuildRows(IEnumerable<IResultRow> rows, IEnumerable<IPresenter> presenters, JsonWriter writer)
        {
            writer.WritePropertyName("rows");
            writer.WriteStartArray();
            foreach (IResultRow row in rows)
            {

                writer.WriteStartArray();
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    var value = presenters.ElementAt(i).Execute(row[i]);
                    writer.WriteValue(value);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray(); //rows
        }
    }
}
