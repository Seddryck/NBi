using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence
{
    public class SentinelLoopXml
    {
        [XmlAttribute("seed")]
        public string Seed { get; set; }

        [XmlAttribute("terminal")]
        public string Terminal { get; set; }

        [XmlAttribute("step")]
        public string Step { get; set; }
    }
}
