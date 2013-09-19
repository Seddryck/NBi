using System.Collections.Generic;
using System.Xml.Serialization;

namespace NBi.Xml.Settings
{
    public class DefaultXml
    {
        [XmlAttribute("apply-to")]
        public SettingsXml.DefaultScope ApplyTo { get; set; }

        [XmlElement ("connectionString")]
        public string ConnectionString { get; set; }

    }
}
