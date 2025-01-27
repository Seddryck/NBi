using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class ParserXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }
}
