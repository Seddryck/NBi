using System.Xml.Serialization;

namespace NBi.Xml.Constraints.EqualTo
{
    public class KeyXml
    {
        [XmlAttribute("index")]
        public int Index { get; set; }
    }
}
