using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items
{
    public class RestHeaderXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
