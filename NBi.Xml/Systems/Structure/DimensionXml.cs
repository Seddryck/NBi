using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class DimensionXml : PerspectiveXml
    {
        public override bool IsStructure()
        {
            return Structure != null;
        }

        public override bool IsMembers()
        {
            return Members != null;
        }

        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        [XmlElement("structure")]
        public StructureXml Structure { get; set; }

        [XmlElement("members")]
        public MembersXml Members { get; set; }

        public override object Instantiate()
        {
            DiscoveryCommand cmd;
            if (Structure == null && Members == null)
                throw new ArgumentNullException();

            if (Structure == null)
            {
                if (string.IsNullOrEmpty(Members.ChildrenOf))
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path);
                else
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path, Members.ChildrenOf);
            }
            else
                cmd = DiscoveryFactory.BuildForPerspective(ConnectionString ?? Default.ConnectionString, Perspective);
            return cmd;
        }

        [XmlIgnore]
        public string Path { get { return string.Format("[{0}]", Caption); } }
    }
}
