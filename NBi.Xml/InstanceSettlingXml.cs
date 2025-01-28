using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml;

[XmlInclude(typeof(InstanceUnique))]
public class InstanceSettlingXml
{
    [XmlElement("local-variable")]
    public InstanceVariableXml Variable { get; set; }

    [XmlElement("derived-variable")]
    public List<DerivedVariableXml> DerivedVariables { get; set; } = new List<DerivedVariableXml>();

    [XmlElement("category")]
    public List<string> Categories { get; set; } = new List<string>();

    [XmlElement("trait")]
    public List<TraitXml> Traits { get; set; } = new List<TraitXml>();

    public static InstanceSettlingXml Unique { get; } = new InstanceUnique();

    public class InstanceUnique : InstanceSettlingXml
    { }
}
