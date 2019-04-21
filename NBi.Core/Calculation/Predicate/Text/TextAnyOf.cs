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
    class TextAnyOf : AbstractTextPredicate
    {
        protected IEnumerable<string> References { get => Reference as IEnumerable<string>; }

        public TextAnyOf(bool not, IScalarResolver reference, StringComparison stringComparison)
            : base(not, reference, stringComparison)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var comparer = StringComparer.Create(CultureInfo.InvariantCulture, StringComparison == StringComparison.InvariantCultureIgnoreCase);

            return References.Contains(x.ToString(), comparer);
        }

        public override string ToString() 
            => $"is within the list of {References.Count()} values ('{(String.Join("', '", References.Take(Math.Min(3, References.Count()))))}'{(References.Count()>3 ? ", ..." : string.Empty)})";
    }
}
