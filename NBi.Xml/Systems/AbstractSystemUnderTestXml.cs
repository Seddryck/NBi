using System.Xml.Serialization;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public abstract object Instantiate();
    }
}
