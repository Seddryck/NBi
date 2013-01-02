using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlIgnore]
        public virtual DefaultXml Default { get; set; }
        [XmlIgnore]
        public virtual SettingsXml Settings { get; set; }

        public AbstractSystemUnderTestXml()
        {
            Default = new DefaultXml();
            Settings = new SettingsXml();
        }

        internal virtual Dictionary<string, string> GetRegexMatch()
        {
            return new Dictionary<string, string>();
        }

        public abstract ICollection<string> GetAutoCategories();
    }
}
