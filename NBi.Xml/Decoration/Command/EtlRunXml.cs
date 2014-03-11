using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using NBi.Xml.Items;

namespace NBi.Xml.Decoration.Command
{
    public class EtlRunXml : DecorationCommandXml, IEtlRunCommand
    {
        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }


        [XmlElement("parameter")]
        public List<EtlParameter> Parameters { get; set; }

        public EtlRunXml()
        {
            Parameters = new List<EtlParameter>();
        }
    }
}
