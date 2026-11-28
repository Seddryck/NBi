using System;
using System.Linq;
using NBi.Core.Members.Ranges;

namespace NBi.Model.Items.Ranges;

public class IntegerRange : ValueRange, IIntegerRange
{
    public int Start { get; set; }
    public int End { get; set; }
    public int Step { get; set; }

    public IntegerRange()
    {
        Step = 1;
    }
}
