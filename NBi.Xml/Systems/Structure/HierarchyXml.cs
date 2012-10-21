using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class HierarchyXml : DimensionXml
    {
        public override bool IsStructure()
        {
            return Structure != null;
        }

        public override bool IsMembers()
        {
            return Members != null;
        }

        [XmlAttribute("dimension")]
        public string Dimension { get; set; }

        public override object Instantiate()
        {
            DiscoveryCommand cmd;
            if (Structure == null)
            {
                if (string.IsNullOrEmpty(Members.ChildrenOf))
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path);
                else
                    cmd = DiscoveryFactory.BuildForMembers(ConnectionString ?? Default.ConnectionString, Perspective, Path, Members.ChildrenOf);
            }
            else
                cmd = DiscoveryFactory.BuildForDimension(ConnectionString ?? Default.ConnectionString, Perspective, Dimension);
            return cmd;
        }

        [XmlIgnore]
        protected string ParentPath { get { return string.Format("[{0}]", Dimension); } }
        [XmlIgnore]
        public string Path { get { return string.Format("{0}.[{1}]", ParentPath, Caption); } }
    }
}
