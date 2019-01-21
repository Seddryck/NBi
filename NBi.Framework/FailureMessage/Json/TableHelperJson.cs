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

namespace NBi.Framework.FailureMessage.Json
{
    class TableHelperJson
    {
        public void Execute(IEnumerable<DataRow> rows, ISampler<DataRow> sampler, JsonWriter writer)
            => Execute(rows, sampler, BuildMetadataFromTable((rows ?? new DataRow[0]).Count()>0 ? rows.ElementAt(0).Table : null), writer);

        private IEnumerable<ColumnMetadata> BuildMetadataFromTable(DataTable table)
        {
            if (table == null)
                yield break;

            foreach (DataColumn column in table.Columns)
            {
                yield return new ColumnMetadata()
                {
                    Role = (ColumnRole)(column.ExtendedProperties["NBi::Role"] ?? ColumnRole.Key),
                    Type = (ColumnType)(column.ExtendedProperties["NBi::Type"] ?? ColumnType.Text),
                    Tolerance = (Tolerance)(column.ExtendedProperties["NBi::Tolerance"]),
                    Rounding = (Rounding)(column.ExtendedProperties["NBi::Rounding"])
                };
            }
        }

        public void Execute(IEnumerable<DataRow> rows, ISampler<DataRow> sampler, IEnumerable<ColumnMetadata> metadata, JsonWriter writer)
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
                var formatters = new List<IPresenter>();

                var columns = sampled.ElementAt(0).Table.Columns;
                for (var i = 0; i < columns.Count; i++)
                {
                    var meta = metadata.ElementAt(i);

                    writer.WriteStartObject();
                    writer.WritePropertyName("position");
                    writer.WriteValue(columns[i].Ordinal);
                    writer.WritePropertyName("name");
                    writer.WriteValue(columns[i].ColumnName);

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

        protected virtual void BuildRows(IEnumerable<DataRow> rows, IEnumerable<IPresenter> presenters, JsonWriter writer)
        {
            writer.WritePropertyName("rows");
            writer.WriteStartArray();
            foreach (DataRow row in rows)
            {

                writer.WriteStartArray();
                for (int i = 0; i < row.ItemArray.Count(); i++)
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

