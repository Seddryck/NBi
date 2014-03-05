using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.WindowsService;

namespace NBi.Xml.Decoration.Check
{
    public class ServiceRunningXml : DecorationCheckXml, IWindowsServiceRunningCheck
    {
        [XmlAttribute("name")]
        public string ServiceName { get; set; }

        [XmlAttribute("timeout-milliseconds")]
        [DefaultValue(5000)]
        public int TimeOut { get; set; }

        public ServiceRunningXml()
        {
            TimeOut = 5000;
        }
    }
}
