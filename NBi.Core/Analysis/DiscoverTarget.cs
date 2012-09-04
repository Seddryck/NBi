using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Analysis
{
    public enum DiscoverTarget
    {
        [XmlEnum(Name = "perspectives")]
        Perspectives,
        [XmlEnum(Name = "measure-groups")]
        MeasureGroups,
        [XmlEnum(Name = "measures")]
        Measures,
        [XmlEnum(Name = "dimensions")]
        Dimensions,
        [XmlEnum(Name = "hierarchies")]
        Hierarchies,
        [XmlEnum(Name = "levels")]
        Levels
    }
}
