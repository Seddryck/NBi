using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class MeasureGroupXml : PerspectiveXml
    {
        public override bool IsStructure()
        {
            return true;
        }

        [XmlAttribute("perspective")]
        public string Perspective { get; set; }

        public override object Instantiate()
        {
            var cmd = DiscoveryFactory.BuildForPerspective(ConnectionString ?? Default.ConnectionString, Perspective);
            return cmd;
        }
    }
}
