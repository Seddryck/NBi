using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Query;

namespace NBi.Xml.Items
{
    public class QueryVariableXml : IQueryVariable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlText]
        public string Value { get; set; }
    }
}
