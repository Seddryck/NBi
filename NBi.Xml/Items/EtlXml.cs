using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;

namespace NBi.Xml.Items
{
    public class EtlXml: QueryableXml, IEtl 
    {
        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("parameter")]
        public List<EtlParameter> Parameters { get; set; }

        public EtlXml()
        {
            Parameters = new List<EtlParameter>();
        }

        public override string GetQuery()
        {
            throw new NotImplementedException();
        }
    }
}
