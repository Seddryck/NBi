using NBi.Core.Scalar.Resolver;
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
        public TextMatchesRegex(bool not, IScalarResolver reference, StringComparison stringComparison)
            : base(not, reference, stringComparison)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var regexOption = StringComparison == StringComparison.InvariantCultureIgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            var regex = new Regex(reference.ToString(), regexOption);
            return regex.IsMatch(x.ToString());
        }

        public override string ToString()
        {
            return $"matches the regex '{Reference}'";
        }
    }
}
