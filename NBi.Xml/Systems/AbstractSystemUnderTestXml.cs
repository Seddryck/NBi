using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public abstract class AbstractSystemUnderTestXml
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        private DefaultXml? _default;
        [XmlIgnore()]
        public virtual DefaultXml? Default
        {
            get { return _default; }
            set
            {
                _default = value;
                if (BaseItem != null)
                    BaseItem.Default = value;
            }
        }
        private SettingsXml? settings;
        [XmlIgnore()]
        public virtual SettingsXml? Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                if (BaseItem != null)
                    BaseItem.Settings = value;
            }
        }

        public abstract BaseItem? BaseItem { get; }

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
