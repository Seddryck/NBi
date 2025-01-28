using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Format;

public enum CurrencyPattern
{
    [XmlEnum(Name = "$n")]
    Prefix = 0,
    [XmlEnum(Name = "n$")]
    Suffix = 1,
    [XmlEnum(Name = "$ n")]
    PrefixSpace = 2,
    [XmlEnum(Name = "n $")]
    SuffixSpace = 3
}
