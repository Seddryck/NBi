using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Settings;

public class DescriptionXml
{
    [XmlText]
    public string Value { get; set; }
}
