using System.Xml.Serialization;

namespace NBi.Xml.Constraints.EqualTo
{
    public class ValueXml
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("type")]
        public TypeChoice Type { get; set; }

        [XmlAttribute("tolerance")]
        public decimal Tolerance { get; set; }

        public enum TypeChoice
        {
            [XmlEnum(Name = "numeric")]
            Numeric = 0,
            [XmlEnum(Name = "text")]
            Text = 1
        }
    }
}
