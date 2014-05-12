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

        public EtlRunXml()
        {
            InternalParameters = new List<EtlParameterXml>();
        }
    }
}
