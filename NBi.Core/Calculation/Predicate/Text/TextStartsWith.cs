using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextStartsWith : AbstractPredicateReference
    {
        public TextStartsWith(object reference) : base(reference)
        { }
        public override bool Apply(object x)
        {
            return x.ToString().StartsWith(Reference.ToString());
        }
    }
}
