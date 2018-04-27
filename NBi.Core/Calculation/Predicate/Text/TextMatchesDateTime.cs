using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesDateTime : CultureSensitiveTextPredicate
    {
        public TextMatchesDateTime(bool not, string culture)
            : base(not, culture)
        { }

        private string Pattern { get => CultureInfo.DateTimeFormat.ShortDatePattern + " " + CultureInfo.DateTimeFormat.LongTimePattern; }

        protected override bool Apply(object x)
        {
            switch (x)
            {
                case string s:
                    return System.DateTime.TryParseExact(s, Pattern, CultureInfo, DateTimeStyles.None, out var result);
                default:
                    return System.DateTime.TryParse(x.ToString(), out var result2);
            }
        }

        public override string ToString()
        {
            var format = Pattern
                .Replace("/", CultureInfo.DateTimeFormat.DateSeparator)
                .Replace(":", CultureInfo.DateTimeFormat.TimeSeparator);

            return $"matches the short date pattern format '{format}'";
        }
    }
}
