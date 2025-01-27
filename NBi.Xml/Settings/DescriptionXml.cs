using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class DescriptionXml
{
    [XmlText]
    public string Value { get; set; }
}
