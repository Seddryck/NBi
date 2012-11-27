using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Analysis.Request
{
    public enum DiscoveryTarget
    {
        Undefined = 0,
        Perspectives,
        MeasureGroups,
        Measures,
        Dimensions,
        Hierarchies,
        Levels,
        Properties
    }
}
