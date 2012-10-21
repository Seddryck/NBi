using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class LevelXml : HierarchyXml
    {

        public override bool IsStructure()
        {
            return Structure != null;
        }

        public override bool IsMembers()
        {
            return Members != null;
        }


        [XmlAttribute("hierarchy")]
        public string Hierarchy { get; set; }

        public override object Instantiate()
        {
            DiscoveryCommand cmd=null;
            if (IsMembers())
            {
                if (string.IsNullOrEmpty(Members.ChildrenOf))
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path);
                else
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path, Members.ChildrenOf);
            }
            else if (IsStructure())
                cmd = DiscoveryFactory.BuildForLevel(ConnectionString ?? Default.ConnectionString, Perspective, Path);

            if (cmd == null)
                throw new ArgumentNullException();

            return cmd;
        }

        [XmlIgnore]
        public string ParentPath { get { return string.Format("{0}.[{1}]",base.ParentPath, Hierarchy);}}

        [XmlIgnore]
        public string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }
    }
}
