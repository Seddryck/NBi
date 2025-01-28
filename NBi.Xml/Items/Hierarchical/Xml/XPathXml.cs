using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Hierarchical.Xml;

public class XPathXml
{
    [XmlAttribute("default-namespace-prefix")]
    public string DefaultNamespacePrefix { get; set; }
    [XmlElement("from")]
    public FromXml From { get; set; }
    [XmlElement("select")]
    public List<SelectXml> Selects { get; set; } = new List<SelectXml>();
}
