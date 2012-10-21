using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    
    public class PerspectiveXml : AbstractSystemUnderTestXml
    {

        public override bool IsStructure()
        {
            return true;
        }

        [XmlAttribute("caption")]
        public string Caption { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        public override object Instantiate()
        {
            var cmd = DiscoveryFactory.BuildForCube(ConnectionString ?? Default.ConnectionString);
            return cmd;
        }
    }
}
