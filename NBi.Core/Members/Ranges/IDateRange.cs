using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges;

public interface IDateRange : IRange
{
    DateTime Start { get; set; }
    DateTime End { get; set; }
    CultureInfo Culture { get; set; }
    string Format { get; set; }
}
