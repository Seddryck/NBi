using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesNumeric : CultureSensitiveTextPredicate
    {
        public TextMatchesNumeric()
            : this(string.Empty)
        { }

        public TextMatchesNumeric(string culture)
            : base (culture)
        { }

        public override bool Apply(object x)
        {
            switch (x)
            {
                case string s:
                    return Double.TryParse(s, NumberStyles.Number & ~NumberStyles.AllowThousands, CultureInfo.NumberFormat, out var result);
                default:
                    return Double.TryParse(x.ToString(), out var result2);
            }
        }

        public override string ToString()
        {
            return $"matches the numeric format.";
        }
    }
}
