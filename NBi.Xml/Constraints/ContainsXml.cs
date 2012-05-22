using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ContainsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("caption")]
        public string Caption { get; set; }
    }
}
