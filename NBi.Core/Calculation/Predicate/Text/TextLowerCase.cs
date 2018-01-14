using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextLowerCase : AbstractPredicate
    {
        public TextLowerCase(bool not)
            : base(not)
        { }

        protected override bool Apply(object x)
        {
            return (x as string).ToLowerInvariant() == (x as string) || (x as string) == "(empty)" || (x as string) == "(null)";
        }

        public override string ToString() => $"is in small letters";
    }
}
