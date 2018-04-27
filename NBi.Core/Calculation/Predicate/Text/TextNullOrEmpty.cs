using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextNullOrEmpty : AbstractPredicate
    {
        public TextNullOrEmpty(bool not)
            : base(not)
        { }

        protected override bool Apply(object x)
        {
            var nullPredicate = new TextNull(false);
            var emptyPredicate = new TextEmpty(false);
            return (nullPredicate.Execute(x) || emptyPredicate.Execute(x));
        }
        public override string ToString() => $"is null or empty";
    }
}
