using System.Xml.Serialization;

namespace NBi.Xml.Constraints
{
    public class ExistsXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        public bool IgnoreCase { get; set; }
    }
}
