using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items
{
    public class QueryVariableXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlText]
        public string Value { get; set; }
    }
}
