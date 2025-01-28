using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NBi.Core.Scalar.Comparer;

public class TextCaseTolerance : TextTolerance
{
    public TextCaseTolerance()
        : base($"ignore-case")
    { }

    public StringComparer Comparison => StringComparer.InvariantCultureIgnoreCase;
}
