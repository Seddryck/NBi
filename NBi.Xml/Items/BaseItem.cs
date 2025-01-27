using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Settings;

namespace NBi.Xml.Items;

public abstract class BaseItem
{
    [XmlIgnore()]
    public virtual DefaultXml? Default { get; set; }
    [XmlIgnore()]
    public virtual SettingsXml? Settings { get; set; }

    public BaseItem()
    {
        Default = new DefaultXml();
        Settings = new SettingsXml();
    }

    [XmlAttribute("connection-string")]
    public string? ConnectionString { get; set; }

    [XmlIgnore]
    [Obsolete("Replaced by connection-string")]
    public string? ConnectionStringOld
    {
        get => ConnectionString;
        set { ConnectionString = value; }
    }

    [XmlAttribute("roles")]
    public string? Roles { get; set; }
}
