using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextContains : AbstractTextPredicate
    {
        public TextContains(object reference, StringComparison stringComparison)
            : base(reference, stringComparison)
        {
        }
        public override bool Apply(object x)
        {
            return x.ToString().IndexOf(Reference.ToString(), StringComparison) >= 0;
        }
    }
}
