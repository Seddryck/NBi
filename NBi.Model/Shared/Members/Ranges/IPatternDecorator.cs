using System;
using System.Linq;

namespace NBi.Core.Members.Ranges;

public interface IPatternDecorator
{
    string Pattern { get; set; }
    PositionValue Position { get; set; }
}

public enum PositionValue
{
    Suffix,
    Prefix
}
