using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class OrderedXml : AbstractConstraintXml
    {
        [XmlAttribute("descending")]
        public bool Descending { get; set; }

        [XmlAttribute("rule")]
        public Order Rule { get; set; }

        public enum Order
        {
            [XmlEnum(Name = "alphabetical")]
            Alphabetical = 0,
            [XmlEnum(Name = "chronological")]
            Chronological = 1,
            [XmlEnum(Name = "numerical")]
            Numerical = 2
        }

    }
}
