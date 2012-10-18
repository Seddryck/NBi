using System;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems
{
    public class MembersXml : AbstractSystemUnderTestXml
    {
        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("children-of")]
        public string ChildrenOf { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var cmd = DiscoveryFactory.BuildForMembers(
                ConnectionString ?? Default.ConnectionString,
                Perspective,
                Path,
                ChildrenOf);
            
            return cmd;
        }
    }
}
