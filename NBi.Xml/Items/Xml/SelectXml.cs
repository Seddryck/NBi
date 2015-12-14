using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Xml
{
    public class SelectXml
    {
        [XmlAttribute("evaluate")]
        public bool Evaluate { get; set; }
        [XmlAttribute("attribute")]
        public string Attribute { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
