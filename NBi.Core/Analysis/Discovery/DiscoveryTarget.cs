using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Analysis.Discovery
{
    public enum DiscoveryTarget
    {
        [XmlEnum(Name = "perspectives")]
        Perspectives,
        //[XmlEnum(Name = "measure-groups")]
        //MeasureGroups,
        [XmlEnum(Name = "measures")]
        Measures,
        [XmlEnum(Name = "dimensions")]
        Dimensions,
        [XmlEnum(Name = "hierarchies")]
        Hierarchies,
        [XmlEnum(Name = "levels")]
        Levels,
        [XmlEnum(Name = "properties")]
        Properties,
        [XmlEnum(Name = "members")]
        Members
    }
}
