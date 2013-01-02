using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints
{
    public abstract class AbstractConstraintXml
    {
        [XmlIgnore]
        public virtual DefaultXml Default { get; set; }
        [XmlIgnore]
        public virtual SettingsXml Settings { get; set; }

        public AbstractConstraintXml()
        {
            Default = new DefaultXml();
            Settings = new SettingsXml();
        }

        [XmlAttribute("not")]
        [DefaultValue(false)]
        public bool Not { get; set; }
        
    }
}
