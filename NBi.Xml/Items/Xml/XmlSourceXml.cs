using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Xml
{
    public class XmlSourceXml : BaseItem
    {
        [XmlAttribute("ignore-namespace")]
        [DefaultValue(false)]
        public bool IgnoreNamespace { get; set; } = false;

        [XmlElement("file")]
        public FileXml File { get; set; }

        [XmlElement("url")]
        public UrlXml Url { get; set; }

        [XmlElement("xpath")]
        public XPathXml XPath { get; set; }
    }
}
