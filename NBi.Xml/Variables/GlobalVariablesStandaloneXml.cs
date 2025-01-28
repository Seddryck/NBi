using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Xml.Variables;

[XmlRoot(ElementName = "variables", Namespace = "")]
public class GlobalVariablesStandaloneXml
{
    [XmlElement("variable")]
    public List<GlobalVariableXml> Variables { get; set; }

    public GlobalVariablesStandaloneXml()
    {
        Variables = new List<GlobalVariableXml>();
    }
}
