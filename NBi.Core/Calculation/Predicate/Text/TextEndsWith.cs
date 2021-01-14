using NBi.Extensibility.Resolving;
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
        public TextEndsWith(bool not, IScalarResolver reference, StringComparison stringComparison)
            : base(not, reference, stringComparison)
        {
        }
        protected override bool ApplyWithReference(object reference, object x)
        {
            return x.ToString().EndsWith(reference.ToString(), StringComparison);
        }


        public override string ToString()
        {
            return $"ends with '{Reference.Execute()}'";
        }
    }
}
