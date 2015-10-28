using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Xml
{
    public class XPathXml
    {
        [XmlElement("from")]
        public FromXml From { get; set; }
        [XmlElement("select")]
        public List<SelectXml> Selects { get; set; }

        public XPathXml()
        {
            Selects = new List<SelectXml>();
        }

    }
}
