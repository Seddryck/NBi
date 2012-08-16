using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace NBi.Xml.Settings
{
    public class SettingsXml
    {
        [XmlElement("default")]
        public List<DefaultXml> Defaults { get; set; }

        public enum DefaultScope
        {
            [XmlEnum(Name = "system-under-test")]
            SystemUnderTest,
            [XmlEnum(Name = "assert")]
            Assert
        }

        public DefaultXml GetDefault(DefaultScope scope)
        {
            return Defaults.SingleOrDefault(d => d.ApplyTo == scope);
        }

    }
}
