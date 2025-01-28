using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core;

public interface IDecorationCondition
{
    bool Validate();
    string? Message { get; }
}
