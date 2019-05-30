using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Xml
{
    public class XmlSourceXml : BaseItem
    {
        [XmlElement("file")]
        public FileXml File { get; set; }

        [XmlElement("url")]
        public UrlXml Url { get; set; }

        [XmlElement("xpath")]
        public XPathXml XPath { get; set; }

        public string GetFile()
        {
            return Settings.BasePath + File.Path;
        }
    }
}
