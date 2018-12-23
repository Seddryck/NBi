using NBi.Core.ResultSet;
using NBi.Xml.Variables.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables
{
    public class InstanceVariableXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public ColumnType Type { get; set; }

        [XmlElement("loop-sentinel")]
        public SentinelLoopXml SentinelLoop { get; set; }
    }
}
