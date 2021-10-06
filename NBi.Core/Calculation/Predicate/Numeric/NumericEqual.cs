using NBi.Core.Scalar.Comparer;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericEqual : AbstractPredicateReference
    {
        public NumericEqual(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool ApplyWithReference(object reference, object x)
        {
            var comparer = new NumericComparer();
            return comparer.Compare(x, reference).AreEqual;
        }
        public override string ToString() => $"is equal to {Reference.Execute()}";
    }
}
