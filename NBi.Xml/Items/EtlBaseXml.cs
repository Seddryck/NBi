using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using System.ComponentModel;

namespace NBi.Xml.Items;

public class EtlBaseXml
{
    protected const int DEFAULT_TIMEOUT = -1;

    [XmlAttribute("version")]
    public string Version { get; set; }
    
    [XmlAttribute("server")]
    public string Server { get; set; }

    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("username")]
    public string UserName { get; set; }

    [XmlAttribute("password")]
    public string Password { get; set; }

    [XmlAttribute("catalog")]
    public string Catalog { get; set; }

    [XmlAttribute("folder")]
    public string Folder { get; set; }

    [XmlAttribute("project")]
    public string Project { get; set; }

    [XmlAttribute("environment")]
    public string Environment { get; set; }

    internal bool IsEmpty()
    {
        return string.IsNullOrEmpty(Server) && string.IsNullOrEmpty(Path) && string.IsNullOrEmpty(Name)
            && string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Catalog)
            && string.IsNullOrEmpty(Folder) && string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Environment)
            && !Is32BitsSpecified && !TimeoutSpecified;
    }

    [XmlAttribute("bits-32")]
    public bool Is32Bits { get; set; }

    [XmlIgnore]
    public bool Is32BitsSpecified { get; set; }

    [XmlAttribute("timeout")]
    public int Timeout { get; set; }

    [XmlIgnore]
    public bool TimeoutSpecified { get; set; }

    public EtlBaseXml()
    {
    }
}
