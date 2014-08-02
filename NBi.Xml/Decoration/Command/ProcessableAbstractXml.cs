using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public abstract class ProcessableAbstractXml : DecorationCommandXml
    {
        [XmlAttribute("name")]
        public string ServiceName { get; set; }

        [XmlAttribute("connection-string")]
        public string ConnectionString { get; set; }

        [XmlAttribute("timeout-milliseconds")]
        [DefaultValue(60000)]
        public int TimeOut { get; set; }

        public ProcessableAbstractXml()
        {
            TimeOut = 60000;
        }
    }
}
