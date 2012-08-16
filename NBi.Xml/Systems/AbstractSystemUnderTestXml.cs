using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public abstract object Instantiate();

        public DefaultXml Default { get; set; }
        
    }
}
