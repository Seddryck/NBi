using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Core.Format;

public interface INumericFormat
{
    int DecimalDigits { get; set; }

    string DecimalSeparator { get; set; }

    string GroupSeparator { get; set; }
}
