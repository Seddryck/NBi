using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Hierarchical.Json
{
    public class JsonSourceXml : BaseItem
    {
        [XmlElement("file")]
        public FileXml File { get; set; }

        [XmlElement("url")]
        public UrlXml Url { get; set; }

        [XmlElement("json-path")]
        public JsonPathXml JsonPath { get; set; }
    }
}
