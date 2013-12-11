using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.Members.Ranges
{
    public class DateRange : IRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public CultureInfo Culture { get; set; }
        public string Format { get; set; }
    }
}
