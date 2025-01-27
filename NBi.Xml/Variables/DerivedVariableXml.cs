using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables;

public class DerivedVariableXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlAttribute("based-on")]
    public string BasedOn { get; set; }
    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Text)]
    public ColumnType ColumnType { get; set; } = ColumnType.Text;
    [XmlElement("script")]
    public ScriptXml Script { get; set; }
}
