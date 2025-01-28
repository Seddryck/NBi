using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

public class TextMultipleMethodsTolerance : TextTolerance
{
    public string Style { get; private set; }
    public string Value { get; private set; }

    public Func<string, string, bool> Implementation { get; private set; }

    public TextMultipleMethodsTolerance(string style, string value, Func<string, string, bool> func)
        : base($"{style} ({value})")
    {
        Style = style;
        Value = value;
        Implementation = func;
    }

    
}
