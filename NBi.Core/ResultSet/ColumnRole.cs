using System.Xml.Serialization;

namespace NBi.Core.ResultSet;

public enum ColumnRole
{
    [XmlEnum(Name = "key")]
    Key = 0,
    [XmlEnum(Name = "value")]
    Value = 1,
    [XmlEnum(Name = "ignore")]
    Ignore = 2
}
