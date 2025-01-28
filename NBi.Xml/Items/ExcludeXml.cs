using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Items;

public class ExcludeXml
{
    [XmlElement("item")]
    public List<string> Items { get; set; }

    [XmlElement("items")]
    public List<PatternXml> Patterns { get; set; }

    public ExcludeXml()
    {
        Items = new List<string>();
    }
}
