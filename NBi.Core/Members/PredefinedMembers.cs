using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Members
{
    public enum PredefinedMembers
    {
        [XmlEnum(Name = "weekdays")]
        DaysOfWeek = 1,
        [XmlEnum(Name = "months")]
        MonthsOfYear = 2
    }
}
