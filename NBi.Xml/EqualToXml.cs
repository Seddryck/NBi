using System.Xml.Serialization;

namespace NBi.Xml
{
    public class EqualToXml : AbstractConstraintXml
    {
        [XmlAttribute("resultSet-File")]
        public string ResultSetFile { get; set; }
    }
}
