using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Decoration.Command;

public class DataManipulationAbstractXml : DecorationCommandXml
{
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
}
