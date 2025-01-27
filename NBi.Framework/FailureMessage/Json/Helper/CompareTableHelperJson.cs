using NBi.Core.Scalar.Presentation;
using NBi.Extensibility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json;

class CompareTableHelperJson : TableHelperJson
{
    protected override void BuildRows(IEnumerable<IResultRow> rows, IEnumerable<IPresenter> presenters, JsonWriter writer)
    {
        writer.WritePropertyName("rows");
        writer.WriteStartArray();
        foreach (var row in rows)
        {
            writer.WriteStartArray();
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                var presenter = presenters.ElementAt(i);
                writer.WriteStartObject();
                writer.WritePropertyName("value");
                writer.WriteValue(presenter.Execute(row[i]));
                if (!string.IsNullOrEmpty(row.GetColumnError(i)))
                {
                    writer.WritePropertyName("expectation");
                    writer.WriteValue(presenter.Execute(row.GetColumnError(i)));
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray(); //rows
    }
}
