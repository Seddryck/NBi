using NBi.Xml.Items.Api.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Hierarchical.Xml;

public class XmlSourceXml : BaseItem
{
    [XmlAttribute("ignore-namespace")]
    [DefaultValue(false)]
    public bool IgnoreNamespace { get; set; } = false;

    [XmlElement("file")]
    public FileXml File { get; set; }

    [XmlElement("url")]
    public UrlXml Url { get; set; }

    [XmlElement("rest")]
    public RestXml Rest { get; set; }

    [XmlElement("xpath")]
    public XPathXml XPath { get; set; }
}
