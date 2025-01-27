using System.ComponentModel;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class ExistsXml : AbstractConstraintXml
{
    [XmlAttribute("ignore-case")]
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }
}
