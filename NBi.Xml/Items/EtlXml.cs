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

        [XmlIgnore]
        public List<EtlParameter> Parameters
        {
            get
            {
                return InternalParameters.ToList<EtlParameter>();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [XmlElement("parameter")]
        public List<EtlParameterXml> InternalParameters { get; set; }

        public EtlXml()
        {
            InternalParameters = new List<EtlParameterXml>();
        }

        public override string GetQuery()
        {
            throw new NotImplementedException();
        }
    }
}
