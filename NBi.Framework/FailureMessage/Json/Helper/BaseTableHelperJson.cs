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

namespace NBi.Framework.FailureMessage.Json.Helper;

abstract class BaseTableHelperJson<T> : Common.Helper.BaseTableHelper<T, JsonWriter>, Common.Helper.ITableHelper<JsonWriter>
{
    public BaseTableHelperJson(IEnumerable<T> rows, IEnumerable<ColumnMetadata> metadata, ISampler<T> sampler)
        : base(rows, metadata, sampler) { }

    public override void Render(JsonWriter writer)
        => RenderTable(writer);


    protected virtual void RenderTable(JsonWriter writer)
    {
        var rows = Rows ?? [];

        writer.WriteStartObject();
        writer.WritePropertyName("total-rows");
        writer.WriteValue(rows.Count);
        if (Sampler.GetResult().Any())
            RenderNonEmptyTable(writer);
        writer.WriteEndObject();
    }

    protected abstract void RenderNonEmptyTable(JsonWriter writer);

    protected void RenderNonEmptyTable(IReadOnlyCollection<T> rows, IEnumerable<ExtendedMetadata> extendedMetadata, ISampler<T> sampler, JsonWriter writer)
    {
        if (sampler.GetIsSampled())
        {
            writer.WritePropertyName("sampled-rows");
            writer.WriteValue(rows.Count - sampler.GetExcludedRowCount());
        }

        writer.WritePropertyName("table");
        writer.WriteStartObject();
        RenderColumns(extendedMetadata, writer);
        RenderRows(rows, extendedMetadata, writer);
        writer.WriteEndObject(); //table
    }

    protected virtual void RenderRows(IEnumerable<T> rows, IEnumerable<ExtendedMetadata> extendedMetadata, JsonWriter writer)
    {
        writer.WritePropertyName("rows");
        writer.WriteStartArray();
        foreach (var row in rows)
            RenderRow(row, extendedMetadata.Select(x => x.Type), writer);
        writer.WriteEndArray(); //rows
    }

    protected abstract void RenderRow(T row, IEnumerable<ColumnType> columnTypes, JsonWriter writer);

    protected virtual void RenderColumns(IEnumerable<ExtendedMetadata> extendedMetadatas, JsonWriter writer)
    {
        writer.WritePropertyName("columns");
        writer.WriteStartArray();
        foreach (var extendedMetadata in extendedMetadatas)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("position");
            writer.WriteValue(extendedMetadata.Ordinal);
            writer.WritePropertyName("name");
            writer.WriteValue(extendedMetadata.Name);

            var cpFormatter = new ColumnPropertiesFormatter();
            writer.WritePropertyName("role");
            writer.WriteValue(cpFormatter.GetRoleText(extendedMetadata.Role));
            writer.WritePropertyName("type");
            writer.WriteValue(cpFormatter.GetTypeText(extendedMetadata.Type));
            if (!Tolerance.IsNullOrNone(extendedMetadata.Tolerance))
            {
                writer.WritePropertyName("tolerance");
                writer.WriteValue(cpFormatter.GetToleranceText(extendedMetadata.Tolerance).Trim());
            }
            if (extendedMetadata.Rounding != null)
            {
                writer.WritePropertyName("rounding");
                writer.WriteValue(cpFormatter.GetRoundingText(extendedMetadata.Rounding));
            }
            writer.WriteEndObject();
        }
        writer.WriteEndArray(); //columns
    }

    protected virtual void RenderCell(object value, ColumnType columnType, JsonWriter writer)
    {
        var factory = new PresenterFactory();
        var formatter = factory.Instantiate(columnType);
        writer.WriteValue(formatter.Execute(value));
    }
}



