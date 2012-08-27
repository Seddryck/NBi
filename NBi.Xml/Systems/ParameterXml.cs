using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Systems
{
    public class ParameterXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public object Value { get; set; }
    }
}
