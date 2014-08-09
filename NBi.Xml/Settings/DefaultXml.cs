using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Settings
{
    public class DefaultXml
    {
        [XmlAttribute("apply-to")]
        public SettingsXml.DefaultScope ApplyTo { get; set; }

        [XmlElement ("connectionString")]
        public string ConnectionString { get; set; }

        [XmlElement("olap-connectionString")]
        public string OlapConnectionString { get; set; }

        [XmlElement("parameter")]
        public List<QueryParameterXml> Parameters { get; set; }

        [XmlElement("variable")]
        public List<QueryTemplateVariableXml> Variables { get; set; }

        public DefaultXml(SettingsXml.DefaultScope applyTo) : this()
        {
            ApplyTo = applyTo;
        }

        public DefaultXml()
        {
            Parameters = new List<QueryParameterXml>();
            Variables = new List<QueryTemplateVariableXml>();
        }

    }
}
