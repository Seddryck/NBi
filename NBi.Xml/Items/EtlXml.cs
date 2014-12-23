using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;

namespace NBi.Xml.Items
{
    public class EtlXml: ExecutableXml, IEtl 
    {
        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }

        [XmlAttribute("catalog")]
        public string Catalog { get; set; }

        [XmlAttribute("folder")]
        public string Folder { get; set; }

        [XmlAttribute("project")]
        public string Project { get; set; }

        [XmlAttribute("bits-32")]
        public bool Is32Bits { get; set; }

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

        //public override string GetQuery()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
