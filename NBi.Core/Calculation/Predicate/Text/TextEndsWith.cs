using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextEndsWith : AbstractTextPredicate
    {
        public TextEndsWith(object reference, StringComparison stringComparison)
            : base(reference, stringComparison)
        {
        }
        public override bool Apply(object x)
        {
            return x.ToString().EndsWith(Reference.ToString(), StringComparison);
        }


        public override string ToString()
        {
            return $"ends with '{Reference}'";
        }
    }
}
