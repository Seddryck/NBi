using System.Xml.Serialization;

namespace NBi.Xml.Settings
{
    public class ReferenceXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("connectionString")]
        public string ConnectionString { get; set; }

    }
}
