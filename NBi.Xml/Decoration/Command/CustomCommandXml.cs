using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Assemblies;

namespace NBi.Xml.Decoration.Command;

public class CustomCommandXml : DecorationCommandXml
{
    [XmlAttribute("assembly-path")]
    public string AssemblyPath { get; set; }

    [XmlAttribute("type")]
    public string TypeName { get; set; }

    [XmlElement("parameter")]
    public List<CustomCommandParameterXml> Parameters { get; set; } = new List<CustomCommandParameterXml>();
}
