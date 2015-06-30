using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Structure
{
    public enum Target
    {
        Undefined = 0,
        Perspectives,
        MeasureGroups,
        DisplayFolders,
        Measures,
        Dimensions,
        Hierarchies,
        Levels,
        Properties,
        Tables,
        Columns,
        Sets,
        Schemas,
        Routines
    }
}
