using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericMoreThanOrEqual : NumericPredicate
    {
        public NumericMoreThanOrEqual(bool not, IScalarResolver reference) : base(not, reference)
        { }

        protected override bool Compare(decimal x, decimal y)
        {
            return x >= y;
        }

        public override string ToString()
        {
            return $"is more, or equal, than {Reference.Execute()}";
        }
    }
}
