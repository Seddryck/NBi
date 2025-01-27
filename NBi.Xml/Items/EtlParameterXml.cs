using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;

namespace NBi.Xml.Items;

public class EtlParameterXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlText]
    public string StringValue { get; set; }
}
