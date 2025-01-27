using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Members.Predefined;

public enum PredefinedMembers
{
    [XmlEnum(Name = "days-of-week")]
    DaysOfWeek = 1,
    [XmlEnum(Name = "months-of-year")]
    MonthsOfYear = 2
}
