using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints
{
    public abstract class AbstractConstraintXml
    {
        private DefaultXml _default;
        public virtual DefaultXml Default
        {
            get { return _default; }
            set
            {
                _default = value;
                if (BaseItem != null)
                    BaseItem.Default = value;
            }
        }
        private SettingsXml settings;
        public virtual SettingsXml Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                if (BaseItem != null)
                    BaseItem.Settings = value;
            }
        }
        
        [XmlIgnore]
        public virtual BaseItem BaseItem 
        { 
            get {return null;} 
        }

        [XmlAttribute("not")]
        public bool Not { get; set; }
        
    }
}
