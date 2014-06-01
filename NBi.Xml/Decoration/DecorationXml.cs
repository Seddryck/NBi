using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Decoration.Command;

namespace NBi.Xml.Decoration
{
    public abstract class DecorationXml
    {
        [XmlElement(Type = typeof(TableLoadXml), ElementName = "table-load"),
        XmlElement(Type = typeof(TableResetXml), ElementName = "table-reset"),
        XmlElement(Type = typeof(ServiceStartXml), ElementName = "service-start"),
        XmlElement(Type = typeof(ServiceStopXml), ElementName = "service-stop"),
        XmlElement(Type = typeof(EtlRunXml), ElementName = "etl-run")
        ]
        public List<DecorationCommandXml> Commands { get; set; }

        public DecorationXml()
        {
            Commands = new List<DecorationCommandXml>();
        }
    }
}
