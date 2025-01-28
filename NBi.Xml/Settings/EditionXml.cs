using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class EditionXml
{
    [XmlAttribute("author")]
    public string Author { get; set; }
    [XmlAttribute("created")]
    public string DateTime { get; set; }
    [XmlElement("update")]
    public List<UpdateXml> Updates { get; set; }
}
