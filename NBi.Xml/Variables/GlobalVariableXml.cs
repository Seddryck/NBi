using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables
{
    public class GlobalVariableXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("script")]
        public ScriptXml Script { get; set; }

    }
}
