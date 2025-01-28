using NBi.Xml.Items;
using NBi.Xml.Variables.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables;

public class GlobalVariableXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlElement("script")]
    public ScriptXml Script { get; set; }

    [XmlElement("query-scalar")]
    public QueryScalarXml QueryScalar { get; set; }

    [XmlElement("environment")]
    public EnvironmentXml Environment { get; set; }

    [XmlElement("custom")]
    public CustomXml Custom { get; set; }
}
