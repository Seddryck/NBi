using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges;

public interface IIntegerRange : IRange
{
    int Start { get; set; }
    int End { get; set; }
    int Step { get; set; }
}
