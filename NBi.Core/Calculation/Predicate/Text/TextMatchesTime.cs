using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesTime : CultureSensitiveTextPredicate
    {
        public TextMatchesTime()
            : this(string.Empty)
        { }

        public TextMatchesTime(string culture)
            : base(culture)
        { }

        public override bool Apply(object x)
        {
            switch (x)
            {
                case string s:
                    return System.DateTime.TryParseExact(s, CultureInfo.DateTimeFormat.ShortTimePattern, CultureInfo, DateTimeStyles.None, out var result);
                default:
                    return System.DateTime.TryParse(x.ToString(), out var result2);
            }
        }

        public override string ToString()
        {
            return $"matches the short time pattern format '{CultureInfo.DateTimeFormat.ShortTimePattern.Replace("/", CultureInfo.DateTimeFormat.TimeSeparator)}'";
        }
    }
}
