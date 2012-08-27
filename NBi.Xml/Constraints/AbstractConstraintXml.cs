using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints
{
    public abstract class AbstractConstraintXml
    {
        public DefaultXml Default { get; set; }

        [XmlAttribute("not")]
        public bool Not { get; set; }
        
    }
}
