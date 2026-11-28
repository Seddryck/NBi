using System;
using System.Linq;
using NBi.Core.Members.Ranges;

namespace NBi.Model.Items.Ranges;

public class PatternIntegerRange : IntegerRange, IPatternDecorator
{
    public PositionValue Position { get; set; }
    public string Pattern { get; set; }

    public PatternIntegerRange()
        : base()
    {}
}
