using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

public class UpdateXml
{
    [XmlElement("contributor")]
    public string Contributor { get; set; }
    [XmlElement("timestamp")]
    public DateTime Timestamp { get; set; }
}
