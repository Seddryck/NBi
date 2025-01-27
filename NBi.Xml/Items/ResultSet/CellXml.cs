using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.ResultSet;

public class CellXml : ICell
{
    [XmlText]
    public string? StringValue { get; set; }

    [XmlIgnore]
    public object Value { get => StringValue!; set { StringValue = value.ToString(); } }

    [XmlAttribute("column-name")]
    public string? ColumnName { get; set; }

    public override string ToString() => StringValue?.ToString() ?? string.Empty;
}
