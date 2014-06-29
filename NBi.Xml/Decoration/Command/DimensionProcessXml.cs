using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class DimensionProcessXml
    {
        [XmlAttribute("name")]
        public string Name {get; set;}
    }
}
