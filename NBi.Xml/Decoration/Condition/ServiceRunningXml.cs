using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Condition
{
    public class ServiceRunningXml : DecorationConditionXml
    {
        [XmlAttribute("name")]
        public string ServiceName { get; set; }

        [XmlAttribute("timeout-milliseconds")]
        [DefaultValue("5000")]
        public string TimeOut { get; set; }

        public ServiceRunningXml()
        {
            TimeOut = "5000";
        }
    }
}
