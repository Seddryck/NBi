using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using NBi.Xml.Items;
using System.IO;
using NBi.Xml.Settings;
using System.ComponentModel;

namespace NBi.Xml.Decoration.Command;

public class SqlRunXml : DecorationCommandXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("connection-string")]
    public string SpecificConnectionString { get; set; }

    [XmlIgnore]
    [Obsolete("Replaced by connection-string")]
    public string SpecificConnectionStringOld
    {
        get => SpecificConnectionString;
        set { SpecificConnectionString = value; }
    }

    [XmlIgnore]
    public string ConnectionString
    {
        get
        {
            if (!string.IsNullOrEmpty(SpecificConnectionString) && SpecificConnectionString.StartsWith("@"))
                return Settings.GetReference(SpecificConnectionString.Remove(0, 1)).ConnectionString.GetValue();
            if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                return SpecificConnectionString;
            if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString.GetValue();
            return string.Empty;
        }
    }

    public SqlRunXml()
    {
        Version = "SqlServer2014";
    }
}
