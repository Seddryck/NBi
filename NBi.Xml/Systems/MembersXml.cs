using System;
using System.Xml.Serialization;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis;

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
            var cmd = new DiscoverCommand(ConnectionString ?? Default.ConnectionString);

            cmd.Perspective = Perspective;
            cmd.Path = string.IsNullOrEmpty(ChildrenOf) ? Path : string.Format("{0}.[{1}]",Path,ChildrenOf);
            cmd.Function = string.IsNullOrEmpty(ChildrenOf) ? "members" : "children";

            return cmd;
        }
    }
}
