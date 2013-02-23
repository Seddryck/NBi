using System.Xml.Serialization;

namespace NBi.Core.ResultSet
{
    public enum ColumnType
    {
        [XmlEnum(Name = "text")]
        Text = 0,
        [XmlEnum(Name = "numeric")]
        Numeric = 1,
        [XmlEnum(Name = "dateTime")]
        DateTime = 2
    }
}
