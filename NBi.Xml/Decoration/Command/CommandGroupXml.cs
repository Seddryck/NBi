using NBi.Xml.Decoration.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class CommandGroupXml : DecorationCommandXml
    {
        [XmlElement(Type = typeof(TableLoadXml), ElementName = "table-load"),
        XmlElement(Type = typeof(TableResetXml), ElementName = "table-reset"),
        XmlElement(Type = typeof(ServiceStartXml), ElementName = "service-start"),
        XmlElement(Type = typeof(ServiceStopXml), ElementName = "service-stop"),
        XmlElement(Type = typeof(EtlRunXml), ElementName = "etl-run")
        ]
        public List<DecorationCommandXml> Commands { get; set; }

        [DefaultValue(true)]
        [XmlAttribute("parallel")]
        public bool Parallel { get; set; }

        [DefaultValue(false)]
        [XmlAttribute("run-once")]
        public bool RunOnce { get; set; }

        public CommandGroupXml()
        {
            Parallel = true;
            RunOnce = false;
            Commands = new List<DecorationCommandXml>();
        }
    }
}
