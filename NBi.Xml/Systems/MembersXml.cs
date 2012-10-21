using System;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems
{
    public class MembersXml
    {
        [XmlAttribute("children-of")]
        public string ChildrenOf { get; set; }
    }
}
