using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextWithinList : AbstractTextPredicate
    {
        protected IEnumerable<string> References { get => Reference as IEnumerable<string>; }

        public TextWithinList(object reference, StringComparison stringComparison)
            : base(reference, stringComparison)
        { }

        public override bool Apply(object x)
        {
            var comparer = StringComparer.Create(CultureInfo.InvariantCulture, StringComparison == StringComparison.InvariantCultureIgnoreCase);

            return References.Contains(x.ToString(), comparer);
        }

        public override string ToString() 
            => $"is within the list of {References.Count()} values ('{(String.Join("', '", References.Take(Math.Min(3, References.Count()))))}'{(References.Count()>3 ? ", ..." : string.Empty)})";
    }
}
