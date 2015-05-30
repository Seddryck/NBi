using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;

namespace NBi.Xml.Items
{
    public class EtlParameterXml: EtlParameter
    {
        [XmlAttribute("name")]
        public override string Name { get; set; }

        [XmlText]
        public override string StringValue { get; set; }
    }
}
