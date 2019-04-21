using NBi.Core.Scalar.Comparer;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextEqual : AbstractPredicateReference
    {
        public TextEqual(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var cpr = new TextComparer();
            return cpr.Compare(x, reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to '{Reference}'";
        }
    }
}
