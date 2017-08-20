using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextMatchesRegex : AbstractTextPredicate
    {
        public TextMatchesRegex(object reference, StringComparison stringComparison)
            : base(reference, stringComparison)
        {
        }
        public override bool Apply(object x)
        {
            var regexOption = StringComparison == StringComparison.InvariantCultureIgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            var regex = new Regex(Reference.ToString(), regexOption);
            return regex.IsMatch(x.ToString());
        }
    }
}
