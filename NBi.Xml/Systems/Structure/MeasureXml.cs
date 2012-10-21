using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Analysis.Discovery;

namespace NBi.Xml.Systems.Structure
{
    public class MeasureXml : MeasureGroupXml
    {
        public override bool IsStructure()
        {
            return true;
        }

        [XmlAttribute("measure-group")]
        public string MeasureGroup { get; set; }

        public override object Instantiate()
        {
            var cmd = DiscoveryFactory.BuildForMeasureGroup(ConnectionString ?? Default.ConnectionString, Perspective, MeasureGroup);
            return cmd;
        }
    }
}
