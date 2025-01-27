using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Settings;

[XmlRoot(ElementName = "settings", Namespace = "")]
public class SettingsStandaloneXml : SettingsXml
{
}
