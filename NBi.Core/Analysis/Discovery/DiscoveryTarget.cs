using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Analysis.Discovery
{
    public enum DiscoveryTarget
    {
        Undefined = 0,
        Perspectives,
        MeasureGroups,
        Measures,
        Dimensions,
        Hierarchies,
        Levels
    }
}
