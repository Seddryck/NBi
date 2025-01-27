using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Condition;

public class FolderExistsConditionXml : DecorationConditionXml
{
    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [DefaultValue(false)]
    [XmlAttribute("not-empty")]
    public bool NotEmpty { get; set; } = false;
}
