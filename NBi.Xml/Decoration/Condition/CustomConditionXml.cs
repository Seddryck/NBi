using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Assemblies;

namespace NBi.Xml.Decoration.Condition;

public class CustomConditionXml : DecorationConditionXml
{
    [XmlAttribute("assembly-path")]
    public string AssemblyPath { get; set; }

    [XmlAttribute("type")]
    public string TypeName { get; set; }

    [XmlElement("parameter")]
    public List<CustomConditionParameterXml> Parameters { get; set; } = new List<CustomConditionParameterXml>();
}
