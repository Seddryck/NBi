using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Model.Xml.Settings;

public class IgnoreXml
{
    [XmlText]
    public string Reason { get; set; }
}
