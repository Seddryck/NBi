using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command;

public abstract class ServiceAbstractXml : DecorationCommandXml
{
    [XmlAttribute("name")]
    public string ServiceName { get; set; }

    [XmlAttribute("timeout-milliseconds")]
    [DefaultValue("5000")]
    public string TimeOut { get; set; }

    public ServiceAbstractXml()
    {
        TimeOut = "5000";
    }
}
