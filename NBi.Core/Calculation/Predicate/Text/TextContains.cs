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
        public TextContains(bool not, object reference, StringComparison stringComparison)
            : base(not, reference, stringComparison)
        {
        }
        protected override bool Apply(object x)
        {
            return x.ToString().IndexOf(Reference.ToString(), StringComparison) >= 0;
        }

        public override string ToString()
        {
            return $"contains the text '{Reference}'";
        }
    }
}
