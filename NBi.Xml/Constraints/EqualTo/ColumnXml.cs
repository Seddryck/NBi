using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Constraints.EqualTo
{
    public class ColumnXml: IColumn
    {
        [XmlAttribute("index")]
        public int Index {get; set;}
        [XmlAttribute("role")]
        public ColumnRole Role{get; set;}
        [XmlAttribute("type")]
        public ColumnType Type{get; set;}
        [XmlAttribute("tolerance")]
        public decimal Tolerance { get; set; }
    }
}
