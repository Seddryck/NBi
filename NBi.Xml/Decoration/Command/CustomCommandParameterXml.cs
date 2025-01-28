using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command;

public class CustomCommandParameterXml
{
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string StringValue { get; set; }
}
