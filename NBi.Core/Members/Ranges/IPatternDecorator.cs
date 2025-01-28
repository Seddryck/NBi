using System;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Members.Ranges;

public interface IPatternDecorator
{
    string Pattern { get; set; }
    PositionValue Position { get; set; }
}

public enum PositionValue
{
    [XmlEnum(Name = "suffix")]
    Suffix,
    [XmlEnum(Name = "prefix")]
    Prefix
}
