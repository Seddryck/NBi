using System;
using System.ComponentModel;
using System.Linq;

namespace NBi.Core.Format;

public interface INumericFormat
{
    int DecimalDigits { get; set; }

    string DecimalSeparator { get; set; }

    string GroupSeparator { get; set; }
}
