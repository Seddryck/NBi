using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Custom
{
    public class CustomScalarXml 
    {
        [XmlAttribute("assembly-path")]
        public string AssemblyPath { get; set; }

        [XmlAttribute("type")]
        public string TypeName { get; set; }

        [XmlElement("parameter")]
        public List<CustomScalarParameterXml> Parameters { get; set; } = new List<CustomScalarParameterXml>();
    }
}
