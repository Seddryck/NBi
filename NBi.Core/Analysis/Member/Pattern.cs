using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NBi.Core.Analysis.Member;

public enum Pattern
{
    [XmlEnum(Name = "start-with")]
    StartWith = 0,
    [XmlEnum(Name = "end-with")]
    EndWith = 1,
    [XmlEnum(Name = "exact")]
    Exact =2,
    [XmlEnum(Name = "contain")]
    Contain = 3
}
