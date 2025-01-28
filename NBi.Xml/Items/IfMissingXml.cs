using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class IfMissingXml
{
    [XmlElement("file")]
    public FileXml File { get; set; } = new FileXml();
}
