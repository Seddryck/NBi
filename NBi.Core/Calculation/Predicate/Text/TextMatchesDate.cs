using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesDate : CultureSensitiveTextPredicate
    {
        public TextMatchesDate(bool not, string culture)
            : base(not, culture)
        { }

        protected override bool Apply(object x)
        {
            switch (x)
            {
                case string s:
                    return System.DateTime.TryParseExact(s, CultureInfo.DateTimeFormat.ShortDatePattern, CultureInfo, DateTimeStyles.None, out var result);
                default:
                    return System.DateTime.TryParse(x.ToString(), out var result2);
            }
        }

        public override string ToString()
        {
            return $"matches the short date pattern format '{CultureInfo.DateTimeFormat.ShortDatePattern.Replace("/", CultureInfo.DateTimeFormat.DateSeparator)}'";
        }
    }
}
